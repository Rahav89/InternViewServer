using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class Intern
    {
        public int Id { get; set; }
        public string First_name { get; set; }

        public string Last_name { get; set; }

        public string Interns_year { get; set; }

        public int Interns_rating { get; set; }


        static public List<Intern> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadIntern();
        }
    }
}