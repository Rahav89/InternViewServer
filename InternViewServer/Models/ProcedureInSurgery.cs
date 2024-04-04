using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class ProcedureInSurgery
    {
        public int Surgery_id { get; set; }
        public int procedure_Id { get; set; }



        static public List<ProcedureInSurgery> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadProcedureInSurgery();
        }
    }
}
