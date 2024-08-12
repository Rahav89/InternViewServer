using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class InternSchedule
    {
        public DateTime DutyDate { get; set; }     // Date
        public int Intern_id { get; set; }
        // Date
        static public List<InternSchedule> GetAllInternsShiftsSchedule()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllInternsShiftsSchedule();
        }
        public int AddInternsShiftsSchedule()
        {
            DBservices dbs = new DBservices();
            return dbs.AddInternsShiftsSchedule(this);
        }

        public bool DeleteInternsShiftsSchedule()
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteInternsShiftsSchedule(this);
        }

        static public List<int> GetInternsOnDutyDayBefore(DateTime GivenDate)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInternsOnDutyDayBefore(GivenDate);
        }
    }


}
