namespace InternViewServer.Models
{
    public class Surgeries
    {
        public int procedure_Id { get; set; }

        public int Case_number { get; set; }

        public int Patient_age { get; set; }

        public DateTime Surgery_date { get; set; }
        public int Difficulty_level { get; set; }
    }
}
