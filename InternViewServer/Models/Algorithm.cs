using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.Sat;
using Microsoft.Extensions.Logging;
using InternViewServer.Models.DAL;
using InternViewServer.Models;
public class Intern_
{
    public int InternId { get; set; }
    public string InternName { get; set; }
    public int InternsRating { get; set; }
    public int InternsYear { get; set; }
}

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
        Intern_ intern,
        Surgeries surgery,
        List<Dictionary<string, object>> syllabusData,
        Dictionary<string, double> weights,
        string role,
        ILogger logger)
    {
        // Skills criteria
        double skillScore = 0.0;
        if (surgery.Difficulty_level == 3)
        {
            if (intern.InternsRating >= 7)
            {
                skillScore = weights["skills"];
            }
        }
        else if (surgery.Difficulty_level == 2)
        {
            if (intern.InternsRating >= 5)
            {
                skillScore = weights["skills"];
            }
        }
        else
        {
            skillScore = weights["skills"];
        }

        // Year criteria
        double yearScore = intern.InternsYear switch
        {
            6 => weights["year"],
            5 => weights["year"] * 0.8,
            4 => weights["year"] * 0.6,
            3 => weights["year"] * 0.4,
            _ => weights["year"] * 0.2
        };

        // Syllabus criteria
        List<double> syllabusScores = new List<double>();
        // GET the Procedures of surgery from the DB
        DBservices _dbServices = new DBservices();
        List<int> procedureOfSurgery = _dbServices.GetProceduresOfSurgery(surgery.Surgery_id);

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

                int need = totalNeeded - totalDone;

                if (need > 0)
                {
                    double completion = ((double)need / totalNeeded) * 100;
                    double syllabusScore = 0.0;
                    if (completion <= 10 || completion >= 80)
                    {
                        syllabusScore = weights["syllabus"];
                    }
                    else if ((completion > 10 && completion <= 30) || (completion >= 60 && completion < 80))
                    {
                        syllabusScore = weights["syllabus"] * 0.6;
                    }
                    else if (completion > 30 && completion < 60)
                    {
                        syllabusScore = weights["syllabus"] * 0.4;
                    }
                    logger.LogInformation($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.Surgery_id} - Procedure {procedure} - Role {role}: completion={completion:F2}%, syllabus_score={syllabusScore}");
                    syllabusScores.Add(syllabusScore);
                }
            }
        }

        double syllabusScoreAvg = syllabusScores.Any() ? syllabusScores.Average() : 0.0;

        // Year and difficulty level criteria
        double yearDifficultyScore = (intern.InternsYear <= 3 && surgery.Difficulty_level <= 2) ||
                                     (intern.InternsYear >= 4 && surgery.Difficulty_level <= 3)
                                     ? weights["year_difficulty"]
                                     : 0.0;

        // Debug output
        logger.LogInformation($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.Surgery_id} - Role {role}: skill_score={skillScore}, year_score={yearScore}, syllabus_score={syllabusScoreAvg}, year_difficulty_score={yearDifficultyScore}");

        // Total score calculation
        double totalScore = skillScore + yearScore + syllabusScoreAvg + yearDifficultyScore;
        double matchScore = totalScore / 10.0; // No rounding

        // Final score output
        logger.LogInformation($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.Surgery_id} - Role {role}: final_match_score={matchScore}");

        return matchScore;
    }

    public static List<MatchResult> CalculateAllMatches(
        List<Intern_> interns,
        List<Surgeries> surgeries,
        Dictionary<int, List<Dictionary<string, object>>> syllabuses,
        Dictionary<string, double> weights,
        ILogger logger)
    {
        List<MatchResult> results = new List<MatchResult>();
        string[] roles = { "main", "first", "second" };

        foreach (var intern in interns)
        {
            if (syllabuses.TryGetValue(intern.InternId, out var internSyllabuses))
            {
                foreach (var surgery in surgeries)
                {
                    foreach (var role in roles)
                    {
                        double matchScore = CalculateMatchScore(intern, surgery, internSyllabuses, weights, role, logger);
                        results.Add(new MatchResult { InternId = intern.InternId, SurgeryId = surgery.Surgery_id, Role = role, MatchScore = matchScore });
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

    public Algorithm(ILogger<Algorithm> logger)
    {
        _logger = logger;
        dbs = new DBservices();
    }

    public List<OptimalAssignment> CalculateOptimalAssignments(string startDate, string endDate)
    {
        // Fetch surgeries within the date range
        var surgeries = dbs.GetSurgeriesByTime(startDate, endDate);
        if (surgeries == null || !surgeries.Any())
        {
            _logger.LogWarning("No surgeries found within the given date range.");
            return new List<OptimalAssignment>();
        }

        // Fetch all interns
        var interns = new List<Intern_>
        {
            new Intern_ { InternId = 1, InternName = "John Doe", InternsRating = 2, InternsYear = 1 },
            new Intern_ { InternId = 2, InternName = "Jane Smith", InternsRating = 6, InternsYear = 4 },
            new Intern_ { InternId = 3, InternName = "Mike Johnson", InternsRating = 7, InternsYear = 3 },
            new Intern_ { InternId = 4, InternName = "Emily Davis", InternsRating = 9, InternsYear = 6 }
        };

        // Fetch syllabus for each intern using the list of dictionaries
        var detailedSyllabuses = new Dictionary<int, List<Dictionary<string, object>>>();
        foreach (var intern in interns)
        {
            var internSyllabus = dbs.GetInternSyllabusForAlgo(intern.InternId);
            detailedSyllabuses[intern.InternId] = internSyllabus;
        }

        // Prepare weights for scoring
        var weights = new Dictionary<string, double>
        {
            { "skills", 25.0 },
            { "year", 25.0 },
            { "syllabus", 25.0 },
            { "year_difficulty", 25.0 }
        };

        // Calculate all matches
        var matchResults = MatchCalculator.CalculateAllMatches(interns, surgeries, detailedSyllabuses, weights, _logger);

        // Log match results for debugging
        _logger.LogInformation("Match Scores Matrix:");
        _logger.LogInformation("\tSurgery-Role:\t{Roles}", string.Join("\t", surgeries.SelectMany(s => new[] { "main", "first", "second" }, (s, role) => $"{s.Surgery_id}-{role}")));

        foreach (var intern in interns)
        {
            var scores = surgeries.SelectMany(surgery => new[] { "main", "first", "second" }
                .Select(role => matchResults.FirstOrDefault(r => r.InternId == intern.InternId && r.SurgeryId == surgery.Surgery_id && r.Role == role)?.MatchScore ?? 0)
                .Select(matchScore => $"{matchScore:F2}")).ToArray();

            _logger.LogInformation("{InternName}\t{Scores}", intern.InternName, string.Join("\t", scores));
        }

        // Prepare variables and constraints for the solution
        CpModel model = new CpModel();
        Dictionary<(int, int, string), IntVar> variables = new Dictionary<(int, int, string), IntVar>();

        List<LinearExpr> objectiveTerms = new List<LinearExpr>();

        foreach (var surgery in surgeries)
        {
            foreach (var role in new[] { "main", "first", "second" })
            {
                foreach (var intern in interns)
                {
                    var matchScore = MatchCalculator.CalculateMatchScore(intern, surgery, detailedSyllabuses[intern.InternId], weights, role, _logger);
                    var variable = model.NewBoolVar($"{intern.InternId}{surgery.Surgery_id}{role}");
                    variables[(intern.InternId, surgery.Surgery_id, role)] = variable;
                    objectiveTerms.Add(variable * (int)(matchScore * 100));
                }
            }
        }

        model.Maximize(LinearExpr.Sum(objectiveTerms.ToArray()));

        foreach (var surgery in surgeries)
        {
            foreach (var intern in interns)
            {
                var roleVariables = new[] { "main", "first", "second" }.Select(role => variables[(intern.InternId, surgery.Surgery_id, role)]).ToArray();
                model.Add(LinearExpr.Sum(roleVariables) <= 1);
            }
        }

        foreach (var surgery in surgeries)
        {
            foreach (var role in new[] { "main", "first", "second" })
            {
                var roleVariables = interns.Select(intern => variables[(intern.InternId, surgery.Surgery_id, role)]).ToArray();
                model.Add(LinearExpr.Sum(roleVariables) == 1);
            }
        }

        CpSolver solver = new CpSolver();
        CpSolverStatus status = solver.Solve(model);

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
                        if (solver.Value(variables[(intern.InternId, surgery.Surgery_id, role)]) == 1)
                        {
                            switch (role)
                            {
                                case "main":
                                    assignment.MainInternId = intern.InternId;
                                    _logger.LogInformation($"  {role}: Intern {intern.InternName} ({intern.InternId})");
                                    break;
                                case "first":
                                    assignment.FirstAssistantInternId = intern.InternId;
                                    _logger.LogInformation($"  {role}: Intern {intern.InternName} ({intern.InternId})");
                                    break;
                                case "second":
                                    assignment.SecondAssistantInternId = intern.InternId;
                                    _logger.LogInformation($"  {role}: Intern {intern.InternName} ({intern.InternId})");
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

        return optimalAssignments;
    }
}


