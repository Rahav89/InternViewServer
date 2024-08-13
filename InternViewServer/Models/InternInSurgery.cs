using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class InternInSurgery
    {
        public int Surgery_id { get; set; }
        public int Intern_id { get; set; }
        public string Intern_role { get; set; }


        //עדכון של מתמחה בניתוח
        //!!!!!!InternInSurgery
        static public bool UpdateOrAddInternInSurgery(InternInSurgery match)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateOrAddInternInSurgery(match);

        }
        //!!!!!!for InternInSurgery
    }


}
