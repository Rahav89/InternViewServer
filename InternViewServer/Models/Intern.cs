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


        static public List<Intern> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadIntern();
        }

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

        static public List<SurgeriesOfIntern> FiveRecentInternSurgeries(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.FiveRecentInternSurgeries(internId);
        }

    }
}