namespace InternViewServer.Models
{
    public class DetailedSyllabusOfIntern
    {
        public string procedureName { get; set; }
        public int category_Id { get; set; }
        public string CategoryName { get; set; }
        public int requiredAsMain { get; set; }
        public int requiredAsFirst { get; set; }
        public int requiredAsSecond { get; set; }
        public int doneAsMain { get; set; }
        public int doneAsFirst { get; set; }
        public int doneAsSecond { get; set; }
        public int categoryRequiredFirst { get; set; }
        public int categoryRequiredSecond { get; set; }
    }
}
