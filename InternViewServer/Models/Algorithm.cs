using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.Sat;
using Microsoft.Extensions.Logging;

public class Intern_
{
    public int InternId { get; set; }
    public string InternName { get; set; }
    public int InternsRating { get; set; }
    public int InternsYear { get; set; }
}

public class Surgery
{
    public int SurgeryId { get; set; }
    public int DifficultyLevel { get; set; }
    public List<int> Procedures { get; set; }
}

public class DetailedSyllabus
{
    public string ProcedureName { get; set; }
    public int ProcedureId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int RequiredAsMain { get; set; }
    public int RequiredAsFirst { get; set; }
    public int RequiredAsSecond { get; set; }
    public int DoneAsMain { get; set; }
    public int DoneAsFirst { get; set; }
    public int DoneAsSecond { get; set; }
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
        Surgery surgery,
        List<DetailedSyllabus> syllabuses,
        Dictionary<string, double> weights,
        string role,
        ILogger logger)
    {
        // Skills criteria
        double skillScore = 0.0;
        if (surgery.DifficultyLevel == 3)
        {
            if (intern.InternsRating >= 7)
            {
                skillScore = weights["skills"];
            }
        }
        else if (surgery.DifficultyLevel == 2)
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
        double yearScore = 0.0;
        if (intern.InternsYear == 6)
        {
            yearScore = weights["year"];
        }
        else if (intern.InternsYear == 5)
        {
            yearScore = weights["year"] * 0.8;
        }
        else if (intern.InternsYear == 4)
        {
            yearScore = weights["year"] * 0.6;
        }
        else if (intern.InternsYear == 3)
        {
            yearScore = weights["year"] * 0.4;
        }
        else
        {
            yearScore = weights["year"] * 0.2;
        }

        // Syllabus criteria
        List<double> syllabusScores = new List<double>();
        foreach (int procedure in surgery.Procedures)
        {
            DetailedSyllabus syllabus = syllabuses.FirstOrDefault(s => s.ProcedureId == procedure);
            if (syllabus != null)
            {
                int totalNeeded = 0;
                int totalDone = 0;

                switch (role.ToLower())
                {
                    case "main":
                        totalNeeded = syllabus.RequiredAsMain;
                        totalDone = syllabus.DoneAsMain;
                        break;
                    case "first":
                        totalNeeded = syllabus.RequiredAsFirst;
                        totalDone = syllabus.DoneAsFirst;
                        break;
                    case "second":
                        totalNeeded = syllabus.RequiredAsSecond;
                        totalDone = syllabus.DoneAsSecond;
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
                    logger.LogInformation($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.SurgeryId} - Procedure {procedure} - Role {role}: completion={completion:F2}%, syllabus_score={syllabusScore}");
                    syllabusScores.Add(syllabusScore);
                }
            }
        }

        double syllabusScoreAvg = syllabusScores.Any() ? syllabusScores.Average() : 0.0;

        // Year and difficulty level criteria
        double yearDifficultyScore = 0.0;
        if (intern.InternsYear <= 3 && surgery.DifficultyLevel <= 2)
        {
            yearDifficultyScore = weights["year_difficulty"];
        }
        else if (intern.InternsYear >= 4 && surgery.DifficultyLevel <= 3)
        {
            yearDifficultyScore = weights["year_difficulty"];
        }

        // Debug output
        logger.LogInformation($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.SurgeryId} - Role {role}: skill_score={skillScore}, year_score={yearScore}, syllabus_score={syllabusScoreAvg}, year_difficulty_score={yearDifficultyScore}");

        // Total score calculation
        double totalScore = skillScore + yearScore + syllabusScoreAvg + yearDifficultyScore;
        double matchScore = totalScore / 10.0; // No rounding

        // Final score output
        logger.LogInformation($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.SurgeryId} - Role {role}: final_match_score={matchScore}");

        return matchScore;
    }

    public static List<MatchResult> CalculateAllMatches(
        List<Intern_> interns,
        List<Surgery> surgeries,
        Dictionary<int, List<DetailedSyllabus>> syllabuses,
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
                        results.Add(new MatchResult { InternId = intern.InternId, SurgeryId = surgery.SurgeryId, Role = role, MatchScore = matchScore });
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

    public Algorithm(ILogger<Algorithm> logger)
    {
        _logger = logger;
    }

    public List<OptimalAssignment> CalculateOptimalAssignments()
    {
        // Create sample data
        var interns = new List<Intern_>
        {
            new Intern_ { InternId = 1, InternName = "John Doe", InternsRating = 2, InternsYear = 1 },
            new Intern_ { InternId = 2, InternName = "Jane Smith", InternsRating = 6, InternsYear = 4 },
            new Intern_ { InternId = 3, InternName = "Mike Johnson", InternsRating = 7, InternsYear = 3 },
            new Intern_ { InternId = 4, InternName = "Emily Davis", InternsRating = 9, InternsYear = 6 }
        };

        var surgeries = new List<Surgery>
        {
            new Surgery { SurgeryId = 101, DifficultyLevel = 3, Procedures = new List<int> { 6, 7 } },
            new Surgery { SurgeryId = 102, DifficultyLevel = 1, Procedures = new List<int> { 11, 13, 15 } },
            new Surgery { SurgeryId = 103, DifficultyLevel = 1, Procedures = new List<int> { 1, 2, 6 } }
        };

        // Create detailed syllabuses for each intern
        var detailedSyllabuses = new Dictionary<int, List<DetailedSyllabus>>();
        foreach (var intern in interns)
        {
            detailedSyllabuses[intern.InternId] = new List<DetailedSyllabus>
            {
                new DetailedSyllabus { ProcedureName = "Procedure 1", ProcedureId = 6, CategoryId = 1007, CategoryName = "Category 1", RequiredAsMain = 5, RequiredAsFirst = 2, RequiredAsSecond = 3, DoneAsMain = 1, DoneAsFirst = 0, DoneAsSecond = 0 },
                new DetailedSyllabus { ProcedureName = "Procedure 2", ProcedureId = 7, CategoryId = 1007, CategoryName = "Category 1", RequiredAsMain = 10, RequiredAsFirst = 5, RequiredAsSecond = 5, DoneAsMain = 2, DoneAsFirst = 0, DoneAsSecond = 0 },
                new DetailedSyllabus { ProcedureName = "Procedure 3", ProcedureId = 9, CategoryId = 1005, CategoryName = "Category 2", RequiredAsMain = 8, RequiredAsFirst = 4, RequiredAsSecond = 4, DoneAsMain = 3, DoneAsFirst = 0, DoneAsSecond = 0 },
                new DetailedSyllabus { ProcedureName = "Procedure 4", ProcedureId = 11, CategoryId = 1005, CategoryName = "Category 2", RequiredAsMain = 10, RequiredAsFirst = 5, RequiredAsSecond = 5, DoneAsMain = 2, DoneAsFirst = 0, DoneAsSecond = 1 },
                new DetailedSyllabus { ProcedureName = "Procedure 5", ProcedureId = 13, CategoryId = 1004, CategoryName = "Category 3", RequiredAsMain = 7, RequiredAsFirst = 3, RequiredAsSecond = 3, DoneAsMain = 3, DoneAsFirst = 0, DoneAsSecond = 0 },
                new DetailedSyllabus { ProcedureName = "Procedure 6", ProcedureId = 15, CategoryId = 1003, CategoryName = "Category 4", RequiredAsMain = 5, RequiredAsFirst = 2, RequiredAsSecond = 3, DoneAsMain = 1, DoneAsFirst = 0, DoneAsSecond = 0 }
            };
        }

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

        // Print header row with surgery-role combinations
        _logger.LogInformation("\tSurgery-Role:\t{Roles}", string.Join("\t", surgeries.SelectMany(s => new[] { "main", "first", "second" }, (s, role) => $"{s.SurgeryId}-{role}")));

        // Log each intern's scores
        foreach (var intern in interns)
        {
            var scores = surgeries.SelectMany(surgery => new[] { "main", "first", "second" }
                .Select(role => matchResults.FirstOrDefault(r => r.InternId == intern.InternId && r.SurgeryId == surgery.SurgeryId && r.Role == role)?.MatchScore ?? 0)
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
                    var variable = model.NewBoolVar($"{intern.InternId}{surgery.SurgeryId}{role}");
                    variables[(intern.InternId, surgery.SurgeryId, role)] = variable;
                    // Objective function: maximize match score
                    objectiveTerms.Add(variable * (int)(matchScore * 100));
                }
            }
        }

        // Define the objective function
        model.Maximize(LinearExpr.Sum(objectiveTerms.ToArray()));

        // Constraint: an intern cannot hold more than one role in the same surgery
        foreach (var surgery in surgeries)
        {
            foreach (var intern in interns)
            {
                var roleVariables = new[] { "main", "first", "second" }.Select(role => variables[(intern.InternId, surgery.SurgeryId, role)]).ToArray();
                model.Add(LinearExpr.Sum(roleVariables) <= 1);
            }
        }

        // Constraint: each role must be filled in each surgery
        foreach (var surgery in surgeries)
        {
            foreach (var role in new[] { "main", "first", "second" })
            {
                var roleVariables = interns.Select(intern => variables[(intern.InternId, surgery.SurgeryId, role)]).ToArray();
                model.Add(LinearExpr.Sum(roleVariables) == 1);
            }
        }

        // Solve the model
        CpSolver solver = new CpSolver();
        CpSolverStatus status = solver.Solve(model);

        var optimalAssignments = new List<OptimalAssignment>();

        if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
        {
            _logger.LogInformation("Optimal assignment:");
            foreach (var surgery in surgeries)
            {
                _logger.LogInformation($"Surgery {surgery.SurgeryId}:");
                var assignment = new OptimalAssignment
                {
                    SurgeryId = surgery.SurgeryId
                };

                foreach (var role in new[] { "main", "first", "second" })
                {
                    foreach (var intern in interns)
                    {
                        if (solver.Value(variables[(intern.InternId, surgery.SurgeryId, role)]) == 1)
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
