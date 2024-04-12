using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class SurgeriesOfIntern
    {
        public int Surgery_id { get; set; }
        public string procedureName { get; set; }
        public string Intern_role { get; set; }
        public int Case_number { get; set; }
        public int Patient_age { get; set; }
        public DateTime Surgery_date { get; set; }
        public int Difficulty_level { get; set; }


    }
}
