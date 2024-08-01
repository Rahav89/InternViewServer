using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class Algorithm_Weights
    {
        public int Id { get; set; }
        public int Skills { get; set; }
        public int YearWeight { get; set; }
        public int YearDifficulty { get; set; }
        public int SyllabusWeight { get; set; }



        //Method to update Algorithm Weights 
        public int Update_Algorithm_Weights()
        {
            DBservices dbs = new DBservices();
            return dbs.Update_Algorithm_Weights(this);
        }

        static public Algorithm_Weights Read_Algorithm_Weights()
        {
            DBservices dbs = new DBservices();
            return dbs.Read_Algorithm_Weights();
        }

    }
}
