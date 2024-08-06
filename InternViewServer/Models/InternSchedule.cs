using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class InternSchedule
    {
        public int ScheduleId { get; set; }     // Unique identifier for each schedule entry
        public int InternId { get; set; }       // Reference to the intern's ID from Interns_Table
        public string DayOfWeek { get; set; }   // Day of the week for the schedule
        public string ShiftPeriod { get; set; } // Shift period (e.g., morning, afternoon)
        public bool Confirmed { get; set; }     // Confirmation status of the assignment


        //static public List<InternSchedule> ReadAllInternSchedule()
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.ReadAllInternSchedule();
        //}
    }
}
