using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class Surgeries
    {
        public int Surgery_id { get; set; }

        public int Case_number { get; set; }

        public int Patient_age { get; set; }

        public DateTime Surgery_date { get; set; }
        public int Difficulty_level { get; set; }

        static public List<Surgeries> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadSurgeries();
        }


        static public List<Surgeries> GetSurgeriesByID(int surgeryID)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSurgeriesByID(surgeryID);
        }

    }
}
