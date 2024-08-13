using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class Intern
    {
        public int Id { get; set; }
        public string Password_i { get; set; }
        public string First_name { get; set; }

        public string Last_name { get; set; }

        public string Interns_year { get; set; }

        public int Interns_rating { get; set; }
        public bool isManager { get; set; }

        public string Email_I { get; set; }


        static public List<Intern> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadIntern();
        }

        public bool AddIntern()
        {
            DBservices dbs = new DBservices();
            if (dbs.addIntern(this))
            {
                return true;
            };
            throw new Exception("Intern ID already exists");
        }

        //לקבל את כל פרטי המתמחה לפי האידי שלו
        static public Intern GetInternByID(int internID)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInternByID(internID);
        }

        //פונקציה של התחברות לפי תעודת זהות וסיסמא
        static public Intern LogIn(int id, string password)
        {
            DBservices dbs = new DBservices();
            Intern internComperByPassID = dbs.LogInInternByIDPass(id, password);

            if (internComperByPassID != null)
            {
                return internComperByPassID;
            }
            throw new Exception("No such intern exists with these details");

        }
        //רשימה של כל הניתוחים של מתמחה
        static public List<Dictionary<string, object>> AllInternSurgeries(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.AllInternSurgeries(internId);
        }

       

        //רשימה של  חמשת הניתוחים של מתמחה
        static public List<SurgeriesOfIntern> FiveRecentInternSurgeries(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.FiveRecentInternSurgeries(internId);
        }
        //רשימה של הסילבוס של מתמחה לפי האידי שלו
        static public List<SyllabusOfIntern> GetSyllabusOfIntern(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSyllabusOfIntern(internId);
        }

        //Method to update intern details
        public int UpdateIntern()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateIntern(this);
        }
        //Set intern password by email
        public static int UpdateInternPassword(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateInternPassword(email, password);
        }

        static public List<InternProcedureCounter> InternProcedureSummary()
        {
            DBservices dbs = new DBservices();
            return dbs.InternProcedureSummary();
        }
        static public List<Dictionary<string, object>> FullDetailedSyllabusOfIntern(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.FullDetailedSyllabusOfIntern(internId);
        }

        //מחזיר את הפרטי ניתוח לפי תעודת זהות ושם פרוצדורה
        static public List<Dictionary<string, object>>GetInternSurgeriesByProcedureName(int internID, string procedureName)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInternSurgeriesByProcedureName(internID,procedureName);
        }


        static public List<Dictionary<string, object>> GetInternSurgeriesByProcedure(int internID, int procedureID)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInternSurgeriesByProcedure(internID, procedureID);
        }

        static public int checkEmailIntern(string email)
        {
            DBservices dbs = new DBservices();
            return dbs.checkEmailIntern(email);
        }

        static public List<Dictionary<string, object>> GetInternsForChat(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInternsForChat(internId);
        }

        //for algo
        static public List<Dictionary<string, object>> GetInternSyllabusForAlgo(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInternSyllabusForAlgo(internId);
        }
        //for algo

    }
}