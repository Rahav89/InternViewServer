//using System;
//using System.Collections.Generic;
//using System.Linq;
//using HungarianAlgorithm;

//public class Intern
//{
//    public int InternId { get; set; }
//    public string InternName { get; set; }
//    public int InternsRating { get; set; }
//    public int InternsYear { get; set; }
//}

//public class Surgery
//{
//    public int SurgeryId { get; set; }
//    public int DifficultyLevel { get; set; }
//    public List<int> Procedures { get; set; }
//}

//public class DetailedSyllabus
//{
//    public string ProcedureName { get; set; }
//    public int ProcedureId { get; set; }
//    public int CategoryId { get; set; }
//    public string CategoryName { get; set; }
//    public int RequiredAsMain { get; set; }
//    public int RequiredAsFirst { get; set; }
//    public int RequiredAsSecond { get; set; }
//    public int DoneAsMain { get; set; }
//    public int DoneAsFirst { get; set; }
//    public int DoneAsSecond { get; set; }
//}

//public class MatchResult
//{
//    public int InternId { get; set; }
//    public int SurgeryId { get; set; }
//    public string Role { get; set; }
//    public double MatchScore { get; set; }
//}

//public class OptimalAssignment
//{
//    public int SurgeryId { get; set; }
//    public int? MainInternId { get; set; }
//    public int? FirstAssistantInternId { get; set; }
//    public int? SecondAssistantInternId { get; set; }
//}

//public static class MatchCalculator
//{
//    public static double CalculateMatchScore(Intern intern, Surgery surgery, List<DetailedSyllabus> syllabuses, Dictionary<string, double> weights, string role)
//    {
//        // Skills criteria
//        double skillScore = 0.0;
//        if (surgery.DifficultyLevel == 3)
//        {
//            if (intern.InternsRating >= 7)
//            {
//                skillScore = weights["skills"];
//            }
//        }
//        else if (surgery.DifficultyLevel == 2)
//        {
//            if (intern.InternsRating >= 5)
//            {
//                skillScore = weights["skills"];
//            }
//        }
//        else
//        {
//            skillScore = weights["skills"];
//        }

//        // Year criteria
//        double yearScore = 0.0;
//        if (intern.InternsYear == 6)
//        {
//            yearScore = weights["year"];
//        }
//        else if (intern.InternsYear == 5)
//        {
//            yearScore = weights["year"] * 0.8;
//        }
//        else if (intern.InternsYear == 4)
//        {
//            yearScore = weights["year"] * 0.6;
//        }
//        else if (intern.InternsYear == 3)
//        {
//            yearScore = weights["year"] * 0.4;
//        }
//        else
//        {
//            yearScore = weights["year"] * 0.2;
//        }

//        // Syllabus criteria
//        List<double> syllabusScores = new List<double>();
//        foreach (int procedure in surgery.Procedures)
//        {
//            DetailedSyllabus syllabus = syllabuses.FirstOrDefault(s => s.ProcedureId == procedure);
//            if (syllabus != null)
//            {
//                int totalNeeded = 0;
//                int totalDone = 0;

//                switch (role.ToLower())
//                {
//                    case "main":
//                        totalNeeded = syllabus.RequiredAsMain;
//                        totalDone = syllabus.DoneAsMain;
//                        break;
//                    case "first":
//                        totalNeeded = syllabus.RequiredAsFirst;
//                        totalDone = syllabus.DoneAsFirst;
//                        break;
//                    case "second":
//                        totalNeeded = syllabus.RequiredAsSecond;
//                        totalDone = syllabus.DoneAsSecond;
//                        break;
//                }

//                int need = totalNeeded - totalDone;

//                if (need > 0)
//                {
//                    double completion = ((double)totalDone / totalNeeded) * 100;
//                    double syllabusScore = 0.0;
//                    if (completion <= 10 || completion >= 80)
//                    {
//                        syllabusScore = weights["syllabus"];
//                    }
//                    else if ((completion > 10 && completion <= 30) || (completion >= 60 && completion < 80))
//                    {
//                        syllabusScore = weights["syllabus"] * 0.6;
//                    }
//                    else if (completion > 30 && completion < 60)
//                    {
//                        syllabusScore = weights["syllabus"] * 0.4;
//                    }
//                    Console.WriteLine($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.SurgeryId} - Procedure {procedure} - Role {role}: completion={completion:F2}%, syllabus_score={syllabusScore}");
//                    syllabusScores.Add(syllabusScore);
//                }
//            }
//        }

//        double syllabusScoreAvg = syllabusScores.Any() ? syllabusScores.Average() : 0.0;

//        // Year and difficulty level criteria
//        double yearDifficultyScore = 0.0;
//        if (intern.InternsYear <= 3 && surgery.DifficultyLevel <= 2)
//        {
//            yearDifficultyScore = weights["year_difficulty"];
//        }
//        else if (intern.InternsYear >= 4 && surgery.DifficultyLevel <= 3)
//        {
//            yearDifficultyScore = weights["year_difficulty"];
//        }

//        // Debug output
//        Console.WriteLine($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.SurgeryId} - Role {role}: skill_score={skillScore}, year_score={yearScore}, syllabus_score={syllabusScoreAvg}, year_difficulty_score={yearDifficultyScore}");

//        // Total score calculation
//        double totalScore = skillScore + yearScore + syllabusScoreAvg + yearDifficultyScore;
//        double matchScore = totalScore / 10.0; // No rounding

//        // Final score output
//        Console.WriteLine($"Intern {intern.InternName} (ID: {intern.InternId}) - Surgery {surgery.SurgeryId} - Role {role}: final_match_score={matchScore}");

//        return matchScore;
//    }

//    public static List<MatchResult> CalculateAllMatches(List<Intern> interns, List<Surgery> surgeries, Dictionary<int, List<DetailedSyllabus>> syllabuses, Dictionary<string, double> weights)
//    {
//        List<MatchResult> results = new List<MatchResult>();
//        string[] roles = { "main", "first", "second" };

//        foreach (var intern in interns)
//        {
//            if (syllabuses.TryGetValue(intern.InternId, out var internSyllabuses))
//            {
//                foreach (var surgery in surgeries)
//                {
//                    foreach (var role in roles)
//                    {
//                        double matchScore = CalculateMatchScore(intern, surgery, internSyllabuses, weights, role);
//                        results.Add(new MatchResult { InternId = intern.InternId, SurgeryId = surgery.SurgeryId, Role = role, MatchScore = matchScore });
//                    }
//                }
//            }
//        }

//        return results;
//    }
//}

//public class Program
//{
//    public static void Main()
//    {
//        // Create sample data
//        var interns = new List<Intern>
//        {
//            new Intern { InternId = 1, InternName = "John Doe", InternsRating = 8, InternsYear = 5 },
//            new Intern { InternId = 2, InternName = "Jane Smith", InternsRating = 6, InternsYear = 4 },
//            new Intern { InternId = 3, InternName = "Mike Johnson", InternsRating = 7, InternsYear = 3 },
//            new Intern { InternId = 4, InternName = "Emily Davis", InternsRating = 9, InternsYear = 6 },
//            new Intern { InternId = 5, InternName = "William Brown", InternsRating = 5, InternsYear = 2 },
//            new Intern { InternId = 6, InternName = "Sophia Wilson", InternsRating = 8, InternsYear = 4 },
//            new Intern { InternId = 7, InternName = "James Moore", InternsRating = 6, InternsYear = 3 },
//            new Intern { InternId = 8, InternName = "Olivia Taylor", InternsRating = 7, InternsYear = 5 },
//            new Intern { InternId = 9, InternName = "Daniel Anderson", InternsRating = 8, InternsYear = 6 },
//            new Intern { InternId = 10, InternName = "Grace Thomas", InternsRating = 5, InternsYear = 2 },
//            new Intern { InternId = 11, InternName = "Liam Harris", InternsRating = 9, InternsYear = 5 },
//            new Intern { InternId = 12, InternName = "Ava Clark", InternsRating = 7, InternsYear = 4 }
//        };

//        var surgeries = new List<Surgery>
//        {
//            new Surgery { SurgeryId = 101, DifficultyLevel = 3, Procedures = new List<int> { 6, 7 } },
//            new Surgery { SurgeryId = 102, DifficultyLevel = 2, Procedures = new List<int> { 11, 13, 15 } },
//            new Surgery { SurgeryId = 103, DifficultyLevel = 1, Procedures = new List<int> { 1, 2, 6 } },
//            new Surgery { SurgeryId = 104, DifficultyLevel = 3, Procedures = new List<int> { 4, 5, 9 } },
//            new Surgery { SurgeryId = 105, DifficultyLevel = 2, Procedures = new List<int> { 3, 8, 13 } },
//            new Surgery { SurgeryId = 106, DifficultyLevel = 1, Procedures = new List<int> { 10, 12, 15 } },
//            new Surgery { SurgeryId = 107, DifficultyLevel = 3, Procedures = new List<int> { 14, 9, 11 } },
//            new Surgery { SurgeryId = 108, DifficultyLevel = 2, Procedures = new List<int> { 17, 19, 6 } },
//            new Surgery { SurgeryId = 109, DifficultyLevel = 1, Procedures = new List<int> { 18, 20, 7 } },
//            new Surgery { SurgeryId = 110, DifficultyLevel = 3, Procedures = new List<int> { 21, 22, 13 } }
//        };

//        // Create detailed syllabuses for each intern
//        var detailedSyllabuses = new Dictionary<int, List<DetailedSyllabus>>();
//        foreach (var intern in interns)
//        {
//            detailedSyllabuses[intern.InternId] = new List<DetailedSyllabus>
//            {
//                new DetailedSyllabus { ProcedureName = "Procedure 1", ProcedureId = 6, CategoryId = 1007, CategoryName = "Category 1", RequiredAsMain = 5, RequiredAsFirst = 2, RequiredAsSecond = 3, DoneAsMain = 1, DoneAsFirst = 0, DoneAsSecond = 0 },
//                new DetailedSyllabus { ProcedureName = "Procedure 2", ProcedureId = 7, CategoryId = 1007, CategoryName = "Category 1", RequiredAsMain = 10, RequiredAsFirst = 5, RequiredAsSecond = 5, DoneAsMain = 2, DoneAsFirst = 0, DoneAsSecond = 0 },
//                new DetailedSyllabus { ProcedureName = "Procedure 3", ProcedureId = 9, CategoryId = 1005, CategoryName = "Category 2", RequiredAsMain = 8, RequiredAsFirst = 4, RequiredAsSecond = 4, DoneAsMain = 3, DoneAsFirst = 0, DoneAsSecond = 0 },
//                new DetailedSyllabus { ProcedureName = "Procedure 4", ProcedureId = 11, CategoryId = 1005, CategoryName = "Category 2", RequiredAsMain = 10, RequiredAsFirst = 5, RequiredAsSecond = 5, DoneAsMain = 2, DoneAsFirst = 0, DoneAsSecond = 1 },
//                new DetailedSyllabus { ProcedureName = "Procedure 5", ProcedureId = 13, CategoryId = 1004, CategoryName = "Category 3", RequiredAsMain = 7, RequiredAsFirst = 3, RequiredAsSecond = 3, DoneAsMain = 3, DoneAsFirst = 0, DoneAsSecond = 0 },
//                new DetailedSyllabus { ProcedureName = "Procedure 6", ProcedureId = 15, CategoryId = 1003, CategoryName = "Category 4", RequiredAsMain = 5, RequiredAsFirst = 2, RequiredAsSecond = 3, DoneAsMain = 1, DoneAsFirst = 0, DoneAsSecond = 0 }
//            };
//        }

//        var weights = new Dictionary<string, double>
//        {
//            { "skills", 25.0 },
//            { "year", 25.0 },
//            { "syllabus", 25.0 },
//            { "year_difficulty", 25.0 }
//        };

//        // Calculate all matches
//        var results = MatchCalculator.CalculateAllMatches(interns, surgeries, detailedSyllabuses, weights);

//        var internIds = results.Select(r => r.InternId).Distinct().ToList();
//        var surgeryRoles = results.Select(r => (r.SurgeryId, r.Role)).Distinct().ToList();

//        // Print the match scores matrix with headers
//        Console.WriteLine("Match Scores Matrix:");
//        Console.Write("\t");
//        foreach (var role in surgeryRoles)
//        {
//            Console.Write($"{role.SurgeryId}-{role.Role}\t");
//        }
//        Console.WriteLine();

//        var matchScoresMatrix = new double[internIds.Count, surgeryRoles.Count];
//        for (int i = 0; i < internIds.Count; i++)
//        {
//            Console.Write(interns.First(x => x.InternId == internIds[i]).InternName + "\t");
//            for (int j = 0; j < surgeryRoles.Count; j++)
//            {
//                var match = results.FirstOrDefault(r => r.InternId == internIds[i] && r.SurgeryId == surgeryRoles[j].SurgeryId && r.Role == surgeryRoles[j].Role);
//                matchScoresMatrix[i, j] = match != null ? match.MatchScore : 0;
//                Console.Write(matchScoresMatrix[i, j] + "\t");
//            }
//            Console.WriteLine();
//        }

//        // Inverting match scores to convert to cost for maximization problem
//        double maxScore = matchScoresMatrix.Cast<double>().Max();
//        var costMatrix = new double[internIds.Count, surgeryRoles.Count];
//        for (int i = 0; i < internIds.Count; i++)
//        {
//            for (int j = 0; j < surgeryRoles.Count; j++)
//            {
//                costMatrix[i, j] = maxScore - matchScoresMatrix[i, j];
//            }
//        }

//        // Ensure the cost matrix is square by adding dummy rows/columns with high cost
//        int size = Math.Max(internIds.Count, surgeryRoles.Count);
//        var squareCostMatrix = new double[size, size];

//        for (int i = 0; i < size; i++)
//        {
//            for (int j = 0; j < size; j++)
//            {
//                if (i < internIds.Count && j < surgeryRoles.Count)
//                {
//                    squareCostMatrix[i, j] = costMatrix[i, j];
//                }
//                else
//                {
//                    squareCostMatrix[i, j] = maxScore; // High cost for dummy entries
//                }
//            }
//        }

//        // Print the square cost matrix with headers for debugging
//        Console.WriteLine("\nSquare Cost Matrix:");
//        Console.Write("\t");
//        for (int j = 0; j < size; j++)
//        {
//            if (j < surgeryRoles.Count)
//                Console.Write($"{surgeryRoles[j].SurgeryId}-{surgeryRoles[j].Role}\t");
//            else
//                Console.Write("Dummy\t");
//        }
//        Console.WriteLine();

//        for (int i = 0; i < size; i++)
//        {
//            if (i < internIds.Count)
//                Console.Write(interns.First(x => x.InternId == internIds[i]).InternName + "\t");
//            else
//                Console.Write("Dummy\t");

//            for (int j = 0; j < size; j++)
//            {
//                Console.Write(squareCostMatrix[i, j] + "\t");
//            }
//            Console.WriteLine();
//        }

//        // Convert double matrix to int matrix for Hungarian Algorithm
//        var intSquareCostMatrix = new int[size, size];
//        for (int i = 0; i < size; i++)
//        {
//            for (int j = 0; j < size; j++)
//            {
//                intSquareCostMatrix[i, j] = (int)Math.Round(squareCostMatrix[i, j]);
//            }
//        }

//        // Run the Hungarian Algorithm
//        var assignment = HungarianAlgorithm.HungarianAlgorithm.FindAssignments(intSquareCostMatrix);

//        // Prepare and print the result
//        var optimalAssignments = new List<OptimalAssignment>();
//        for (int i = 0; i < assignment.Length; i++)
//        {
//            if (assignment[i] < surgeryRoles.Count)
//            {
//                var roleIndex = assignment[i];
//                var internIndex = i;

//                if (internIndex >= internIds.Count || roleIndex >= surgeryRoles.Count)
//                {
//                    continue;
//                }

//                var role = surgeryRoles[roleIndex].Role;
//                var surgeryId = surgeryRoles[roleIndex].SurgeryId;

//                var existingAssignment = optimalAssignments.FirstOrDefault(x => x.SurgeryId == surgeryId);
//                if (existingAssignment == null)
//                {
//                    existingAssignment = new OptimalAssignment { SurgeryId = surgeryId };
//                    optimalAssignments.Add(existingAssignment);
//                }

//                switch (role)
//                {
//                    case "main":
//                        existingAssignment.MainInternId = internIds[internIndex];
//                        break;
//                    case "first":
//                        existingAssignment.FirstAssistantInternId = internIds[internIndex];
//                        break;
//                    case "second":
//                        existingAssignment.SecondAssistantInternId = internIds[internIndex];
//                        break;
//                }
//            }
//        }

//        // Print the result
//        Console.WriteLine("\nOptimal Assignments:");
//        foreach (var assignmentResult in optimalAssignments)
//        {
//            Console.WriteLine($"Surgery {assignmentResult.SurgeryId}: Main Intern {assignmentResult.MainInternId}, First Assistant Intern {assignmentResult.FirstAssistantInternId}, Second Assistant Intern {assignmentResult.SecondAssistantInternId}");
//        }
//    }
//}
