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

        public string Hospital_name { get; set; }
        static public List<Dictionary<string, object>> GetAllSurgeriesWithProcedures()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllSurgeriesWithProcedures();
        }
        
        static public List<Surgeries> GetSurgeriesByID(int surgeryID)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSurgeriesByID(surgeryID);
        }
        public int AddSurgery()
        {

            DBservices dbs = new DBservices();
            int addResult = dbs.AddSurgery(this);

            switch (addResult)
            {
                case 1:
                    // Success: return success response
                    return 1;
                case -1:
                    // Error: Conflict within two hours
                    return -1; //("Cannot schedule a surgery within two hours of another surgery at the same hospital.");
                case -2:
                    // Error: More than two surgeries on the same day
                    return -2;// ("Cannot schedule more than two surgeries on the same day at the same hospital.");
                default:
                    // General failure or no rows affected
                    throw new Exception("Failed to add surgery. Please check your data.");
            }

        }
        //static public List<Dictionary<string, object>> GetAllSurgeries()
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.GetAllSurgeries();
        //}
        //static public List<Dictionary<string, object>> GetFutureSurgeries()
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.GetFutureSurgeries();
        //}

        static public List<Dictionary<string, object>> GetSurgeryRoles(int surgery_id)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSurgeryRoles(surgery_id);
        }

        //for algorithm
        static public List<Surgeries> GetSurgeriesByTime(string startDate, string endDate)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSurgeriesByTime(startDate , endDate);
        }

        static public List<int> GetProceduresOfSurgery(int SurgeryId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetProceduresOfSurgery(SurgeryId);
        }
        //for algorithm
    }
}
