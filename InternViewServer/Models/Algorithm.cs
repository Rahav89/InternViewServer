//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//namespace InternViewServer.Models
//{

//    public class Algorithm
//    {
//        public static double CalculateMatchScore(Intern intern, Surgeries surgery, List<Syllabus> syllabuses, Dictionary<string, double> weights)
//        {
//            // Skills criteria
//            double skillScore = 0.0;
//            if (surgery.DifficultyLevel == 3)
//            {
//                if (intern.InternsRating >= 7)
//                {
//                    skillScore = weights["skills"];
//                }
//            }
//            else if (surgery.DifficultyLevel == 2)
//            {
//                if (intern.InternsRating >= 5)
//                {
//                    skillScore = weights["skills"];
//                }
//            }
//            else
//            {
//                skillScore = weights["skills"]; // אם הניתוח הוא לא רמה 3 ולא רמה 2, הוא בוודאי רמה 1
//            }

//            // Year criteria
//            double yearScore = 0.0;
//            switch (intern.InternsYear)
//            {
//                case 6:
//                    yearScore = weights["year"];
//                    break;
//                case 5:
//                    yearScore = weights["year"] * 0.8;
//                    break;
//                case 4:
//                    yearScore = weights["year"] * 0.6;
//                    break;
//                case 3:
//                    yearScore = weights["year"] * 0.4;
//                    break;
//                default:
//                    yearScore = weights["year"] * 0.2; // אם שנת ההתמחות לא 6, 5, 4 או 3, היא בוודאי 2 או 1
//                    break;
//            }

//            // Syllabus criteria
//            List<double> syllabusScores = new List<double>();
//            foreach (var procedure in surgery.Procedures)
//            {
//                var syllabus = syllabuses.FirstOrDefault(s => s.SurgeryId == surgery.SurgeryId && s.ProcedureId == procedure);
//                if (syllabus != null && syllabus.Need > 0)
//                {
//                    double completion = ((syllabus.TotalNeeded - syllabus.Need) / (double)syllabus.TotalNeeded) * 100;
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
//                    syllabusScores.Add(syllabusScore);
//                }
//            }

//            double syllabusScoreAverage = syllabusScores.Any() ? syllabusScores.Average() : 0.0;

//            // Year and difficulty level criteria
//            double yearDifficultyScore = 0.0;
//            if (intern.InternsYear <= 3 && surgery.DifficultyLevel <= 2)
//            {
//                yearDifficultyScore = weights["year_difficulty"];
//            }
//            else if (intern.InternsYear >= 4 && surgery.DifficultyLevel <= 3)
//            {
//                yearDifficultyScore = weights["year_difficulty"];
//            }

//            // חישוב הניקוד הסופי מתוך 100
//            double totalScore = skillScore + yearScore + syllabusScoreAverage + yearDifficultyScore;

//            //וחלוקה ב 10 עיגול התוצאה הסופית למספר אחד אחרי הנקודה
//            double matchScore = Math.Round(totalScore / 10, 1);

//            return matchScore;
//        }
//    }
//}

