using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class InternSchedule
    {
        public DateTime DutyDate { get; set; }     // Unique identifier for each schedule entry
        public int Intern_id { get; set; }       // Reference to the intern's ID from Interns_Table

        static public List<InternSchedule> GetAllInternsDutySchedule()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllInternsDutySchedule();
        }
        public int AddInternDutySchedule()
        {
            DBservices dbs = new DBservices();
            return dbs.AddInternDutySchedule(this);
        }
    }

}
