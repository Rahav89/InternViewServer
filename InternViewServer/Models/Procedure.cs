using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class Procedure
    {
        public int procedure_Id { get; set; }
        public string procedureName { get; set; }
        public int category_Id { get; set; }
        public int quantityAsMain { get; set; }
        public int quantityAsFirst { get; set; }
        public int quantityAsSecond { get; set; }

        static public List<Procedure> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadProcedure();
        }

        static public List<Procedure> GetAllprocedureName ()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllprocedureName();
        }

    }
}
