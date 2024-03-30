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
    }
}
