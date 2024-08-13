using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.Sat;
using Microsoft.Extensions.Logging;
using InternViewServer.Models.DAL;
using InternViewServer.Models;

public class MatchResult
{
    public int InternId { get; set; }
    public int SurgeryId { get; set; }
    public string Role { get; set; }
    public double MatchScore { get; set; }
}

public class OptimalAssignment
{
    public int SurgeryId { get; set; }
    public int? MainInternId { get; set; }
    public int? FirstAssistantInternId { get; set; }
    public int? SecondAssistantInternId { get; set; }
}

public static class MatchCalculator
{
    public static double CalculateMatchScore(
        Intern intern,
        Surgeries surgery,
        List<Dictionary<string, object>> syllabusData,
        Algorithm_Weights weights,
        string role,
        ILogger logger)
    {
        // Null check for intern and surgery
        if (intern == null || surgery == null)
        {
            logger.LogError("Intern or surgery object is null.");
            return 0.0;
        }

        // Null check for syllabusData
        if (syllabusData == null || syllabusData.Count == 0)
        {
            logger.LogWarning($"No syllabus data found for intern {intern.Id} during surgery {surgery.Surgery_id}.");
            return 0.0;
        }

        // Safely parse Interns_year as a date and calculate years of training
        if (!DateTime.TryParse(intern.Interns_year, out DateTime startYear))
        {
            logger.LogError($"Invalid date format for Interns_year '{intern.Interns_year}' for intern {intern.Id}.");
            return 0.0; // Return a default score or handle as needed
        }
        int internYears = DateTime.Now.Year - startYear.Year;

        // Skills criteria
        double skillScore = 0.0;
        if (surgery.Difficulty_level == 3)
        {
            if (intern.Interns_rating >= 7)
            {
                skillScore = weights.Skills;
            }
        }
        else if (surgery.Difficulty_level == 2)
        {
            if (intern.Interns_rating >= 5)
            {
                skillScore = weights.Skills;
            }
        }
        else
        {
            skillScore = weights.Skills;
        }

        // Year criteria
        double yearScore = internYears switch
        {
            6 => weights.YearWeight,
            5 => weights.YearWeight * 0.8,
            4 => weights.YearWeight * 0.6,
            3 => weights.YearWeight * 0.4,
            _ => weights.YearWeight * 0.2
        };

        // Syllabus criteria
        List<double> syllabusScores = new List<double>();
        DBservices _dbServices = new DBservices();
        List<int> procedureOfSurgery = _dbServices.GetProceduresOfSurgery(surgery.Surgery_id) ?? new List<int>();

        foreach (int procedure in procedureOfSurgery)
        {
            var syllabus = syllabusData.FirstOrDefault(s => (int)s["procedure_Id"] == procedure);
            if (syllabus != null)
            {
                int totalNeeded = 0;
                int totalDone = 0;

                switch (role.ToLower())
                {
                    case "main":
                        totalNeeded = (int)syllabus["requiredAsMain"];
                        totalDone = (int)syllabus["doneAsMain"];
                        break;
                    case "first":
                        totalNeeded = (int)syllabus["requiredAsFirst"];
                        totalDone = (int)syllabus["doneAsFirst"];
                        break;
                    case "second":
                        totalNeeded = (int)syllabus["requiredAsSecond"];
                        totalDone = (int)syllabus["doneAsSecond"];
                        break;
                }

                // Ensure no division by zero
                if (totalNeeded > 0)
                {
                    int need = totalNeeded - totalDone;
                    double completion = ((double)totalDone / totalNeeded) * 100;
                    double syllabusScore = 0.0;
                    if (completion <= 10 || completion >= 80)
                    {
                        syllabusScore = weights.SyllabusWeight;
                    }
                    else if ((completion > 10 && completion <= 30) || (completion >= 60 && completion < 80))
                    {
                        syllabusScore = weights.SyllabusWeight * 0.6;
                    }
                    else if (completion > 30 && completion < 60)
                    {
                        syllabusScore = weights.SyllabusWeight * 0.4;
                    }
                    logger.LogInformation($"Intern {intern.First_name} {intern.Last_name} (ID: {intern.Id}) - Surgery {surgery.Surgery_id} - Procedure {procedure} - Role {role}: completion={completion:F2}%, syllabus_score={syllabusScore}");
                    syllabusScores.Add(syllabusScore);
                }
            }
        }

        double syllabusScoreAvg = syllabusScores.Any() ? syllabusScores.Average() : 0.0;

        // Year and difficulty level criteria
        double yearDifficultyScore = (internYears <= 3 && surgery.Difficulty_level <= 2) ||
                                     (internYears >= 4 && surgery.Difficulty_level <= 3)
                                     ? weights.YearDifficulty
                                     : 0.0;

        // Debug output
        logger.LogInformation($"Intern {intern.First_name} {intern.Last_name} (ID: {intern.Id}) - Surgery {surgery.Surgery_id} - Role {role}: skill_score={skillScore}, year_score={yearScore}, syllabus_score={syllabusScoreAvg}, year_difficulty_score={yearDifficultyScore}");

        // Total score calculation
        double totalScore = skillScore + yearScore + syllabusScoreAvg + yearDifficultyScore;
        double matchScore = totalScore / 10.0; // No rounding

        // Final score output
        logger.LogInformation($"Intern {intern.First_name} {intern.Last_name} (ID: {intern.Id}) - Surgery {surgery.Surgery_id} - Role {role}: final_match_score={matchScore}");

        return matchScore;
    }

    public static List<MatchResult> CalculateAllMatches(
        List<Intern> interns,
        List<Surgeries> surgeries,
        Dictionary<int, List<Dictionary<string, object>>> syllabuses,
        Algorithm_Weights weights,
        ILogger logger)
    {
        List<MatchResult> results = new List<MatchResult>();
        string[] roles = { "main", "first", "second" };

        foreach (var surgery in surgeries)
        {
            // Get interns on duty the day before the surgery
            DBservices dbs = new DBservices();
            List<int> internsOnDutyDayBefore = dbs.GetInternsOnDutyDayBefore(surgery.Surgery_date);

            foreach (var intern in interns)
            {
                // Check if the intern is on duty the day before the surgery
                bool isInternOnDutyDayBefore = internsOnDutyDayBefore.Contains(intern.Id);

                if (syllabuses.TryGetValue(intern.Id, out var internSyllabuses))
                {
                    foreach (var role in roles)
                    {
                        double matchScore;

                        if (isInternOnDutyDayBefore)
                        {
                            // Assign a low score if the intern is on duty the day before
                            matchScore = -1.0; // Or 0.0, depending on preference
                        }
                        else
                        {
                            // Calculate the actual match score
                            matchScore = CalculateMatchScore(intern, surgery, internSyllabuses, weights, role, logger);
                        }

                        results.Add(new MatchResult { InternId = intern.Id, SurgeryId = surgery.Surgery_id, Role = role, MatchScore = matchScore });
                    }
                }
            }
        }

        return results;
    }
}

public class Algorithm
{
    private readonly ILogger<Algorithm> _logger;
    private readonly DBservices dbs;

    // Define the role constants
    const string MAIN = "מנתח ראשי";
    const string FIRST_ASSISTANT = "עוזר ראשון";
    const string SECOND_ASSISTANT = "עוזר שני";

    public Algorithm(ILogger<Algorithm> logger)
    {
        _logger = logger;
        dbs = new DBservices();
    }

    public List<OptimalAssignment> CalculateOptimalAssignments(string startDate, string endDate)
    {
        var surgeries = dbs.GetSurgeriesByTime(startDate, endDate);
        if (surgeries == null || !surgeries.Any())
        {
            _logger.LogWarning("No surgeries found within the given date range.");
            return new List<OptimalAssignment>();
        }

        var interns = dbs.ReadIntern();
        if (interns == null || !interns.Any())
        {
            _logger.LogWarning("No interns found.");
            return new List<OptimalAssignment>();
        }

        Algorithm_Weights weights = dbs.Read_Algorithm_Weights();

        var detailedSyllabuses = new Dictionary<int, List<Dictionary<string, object>>>();
        foreach (var intern in interns)
        {
            var internSyllabus = dbs.GetInternSyllabusForAlgo(intern.Id);
            if (internSyllabus != null)
            {
                detailedSyllabuses[intern.Id] = internSyllabus;
            }
        }

        var matchResults = MatchCalculator.CalculateAllMatches(interns, surgeries, detailedSyllabuses, weights, _logger);

        // Log the match scores matrix
        _logger.LogInformation("Match Scores Matrix:");
        _logger.LogInformation("\tSurgery-Role:\t{Roles}", string.Join("\t", surgeries.SelectMany(s => new[] { "main", "first", "second" }, (s, role) => $"{s.Surgery_id}-{role}")));

        foreach (var intern in interns)
        {
            var scores = surgeries.SelectMany(surgery => new[] { "main", "first", "second" }
                .Select(role => matchResults.FirstOrDefault(r => r.InternId == intern.Id && r.SurgeryId == surgery.Surgery_id && r.Role == role)?.MatchScore ?? 0)
                .Select(matchScore => $"{matchScore:F2}")).ToArray();

            _logger.LogInformation("{Internid}\t{Scores}", $"{intern.Id}", string.Join("\t", scores));
        }

        CpModel model = new CpModel();
        Dictionary<(int, int, string), IntVar> variables = new Dictionary<(int, int, string), IntVar>();

        List<LinearExpr> objectiveTerms = new List<LinearExpr>();

        foreach (var surgery in surgeries)
        {
            foreach (var role in new[] { "main", "first", "second" })
            {
                foreach (var intern in interns)
                {
                    var matchScore = matchResults.FirstOrDefault(r => r.InternId == intern.Id && r.SurgeryId == surgery.Surgery_id && r.Role == role)?.MatchScore ?? 0;
                    var variable = model.NewBoolVar($"{intern.Id}{surgery.Surgery_id}{role}");
                    variables[(intern.Id, surgery.Surgery_id, role)] = variable;
                    if (matchScore > 0)
                    {
                        objectiveTerms.Add(variable * (int)(matchScore * 100));
                    }
                }
            }
        }

        model.Maximize(LinearExpr.Sum(objectiveTerms.ToArray()));

        foreach (var surgery in surgeries)
        {
            foreach (var intern in interns)
            {
                var roleVariables = new[] { "main", "first", "second" }.Select(role => variables[(intern.Id, surgery.Surgery_id, role)]).ToArray();
                model.Add(LinearExpr.Sum(roleVariables) <= 1);
            }
        }

        foreach (var surgery in surgeries)
        {
            foreach (var role in new[] { "main", "first", "second" })
            {
                var roleVariables = interns.Select(intern => variables[(intern.Id, surgery.Surgery_id, role)]).ToArray();
                model.Add(LinearExpr.Sum(roleVariables) == 1);
            }
        }

        CpSolver solver = new CpSolver();
        CpSolverStatus status = solver.Solve(model);

        // הוספת לוג להדפסת הסטטוס של הפתרון
        _logger.LogInformation($"Solver status: {status}");

        var optimalAssignments = new List<OptimalAssignment>();

        if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
        {
            _logger.LogInformation("Optimal assignment:");
            foreach (var surgery in surgeries)
            {
                _logger.LogInformation($"Surgery {surgery.Surgery_id}:");
                var assignment = new OptimalAssignment
                {
                    SurgeryId = surgery.Surgery_id
                };

                foreach (var role in new[] { "main", "first", "second" })
                {
                    foreach (var intern in interns)
                    {
                        if (solver.Value(variables[(intern.Id, surgery.Surgery_id, role)]) == 1)
                        {
                            switch (role)
                            {
                                case "main":
                                    assignment.MainInternId = intern.Id;
                                    _logger.LogInformation($"  {role}: Intern {intern.First_name} {intern.Last_name} ({intern.Id})");
                                    break;
                                case "first":
                                    assignment.FirstAssistantInternId = intern.Id;
                                    _logger.LogInformation($"  {role}: Intern {intern.First_name} {intern.Last_name} ({intern.Id})");
                                    break;
                                case "second":
                                    assignment.SecondAssistantInternId = intern.Id;
                                    _logger.LogInformation($"  {role}: Intern {intern.First_name} {intern.Last_name} ({intern.Id})");
                                    break;
                            }
                        }
                    }
                }

                optimalAssignments.Add(assignment);
            }
        }
        else
        {
            _logger.LogWarning("No solution found.");
        }

        // Loop through each assignment and update the database - UpdateOrAddInternInSurgery
        foreach (var assignment in optimalAssignments)
        {
            if (assignment.MainInternId.HasValue)
            {
                var mainMatch = new InternInSurgery
                {
                    Surgery_id = assignment.SurgeryId,
                    Intern_id = assignment.MainInternId.Value,
                    Intern_role = MAIN
                };
                dbs.UpdateOrAddInternInSurgery(mainMatch);
            }

            if (assignment.FirstAssistantInternId.HasValue)
            {
                var firstAssistantMatch = new InternInSurgery
                {
                    Surgery_id = assignment.SurgeryId,
                    Intern_id = assignment.FirstAssistantInternId.Value,
                    Intern_role = FIRST_ASSISTANT
                };
                dbs.UpdateOrAddInternInSurgery(firstAssistantMatch);
            }

            if (assignment.SecondAssistantInternId.HasValue)
            {
                var secondAssistantMatch = new InternInSurgery
                {
                    Surgery_id = assignment.SurgeryId,
                    Intern_id = assignment.SecondAssistantInternId.Value,
                    Intern_role = SECOND_ASSISTANT
                };
                dbs.UpdateOrAddInternInSurgery(secondAssistantMatch);
            }
        }

        return optimalAssignments;
    }
}
