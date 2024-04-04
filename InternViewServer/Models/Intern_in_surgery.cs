using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class Intern_in_surgery
    {
       // public int id { get; set; }
        public int Surgery_id { get; set; }

        public int Intern_id { get; set; }

        public string Intern_role { get; set; }
        static public List<Intern_in_surgery> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadIntern_in_surgery();
        }

    }
}
