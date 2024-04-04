using InternViewServer.Models.DAL;

namespace InternViewServer.Models
{
    public class Category
    {
        public int Category_Id { get; set; }
        public string CategoryName { get; set; }

        public int quantityAsFirst { get; set; }

        public int quantityAsSecond { get; set; }


        static public List<Category> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadCategory();
        }
    }
}
