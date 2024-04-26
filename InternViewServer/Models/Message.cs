using InternViewServer.Models.DAL;
namespace InternViewServer.Models
{
    public class Message
    {
        public int messages_id { get; set; }
        public int from_id { get; set; }
        public int to_id { get; set; }
        public string content { get; set; }
        public DateTime messages_date { get; set; }


        static public List<Dictionary<string, object>> GetInternsForChat(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInternsForChat(internId);
        }

        static public List<Dictionary<string, object>> GetLastMessagesForIntern(int internId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetLastMessagesForIntern(internId);
        }

        static public List<Message> GetChatWithPartner(int internId, int intern_Partner_id)
        {
            DBservices dbs = new DBservices();
            return dbs.GetChatWithPartner(internId, intern_Partner_id);
        }

        public bool AddNewMessage()
        {
            DBservices dbs = new DBservices();// יצירת אובייקט חדש
            int rowsEffected = dbs.AddNewMessage(this);
            if (rowsEffected == 1)
            {
                return true;// הוספה של יוזר חדש כי השתנתה שורה
            }
            return false;
        }

    }
}
