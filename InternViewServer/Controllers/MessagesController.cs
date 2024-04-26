using InternViewServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        // GET: api/<Messages>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/<Messages>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Messages>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Messages>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Messages>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        // GET: api/<Messages>
        [HttpGet]
        [Route("GetInternsForChat")]
        public List<Dictionary<string, object>> GetInternsForChat(int id)
        {
            return Message.GetInternsForChat(id);
        }

        // GET: api/<Messages>
        [HttpGet]
        [Route("GetLastMessagesForIntern")]
        public List<Dictionary<string, object>> GetLastMessagesForIntern(int id)
        {
            return Message.GetLastMessagesForIntern(id);
        }

        // GET: api/<Messages>
        [HttpGet]
        [Route("GetChatWithPartner")]
        public List<Message> GetChatWithPartner(int idIntern, int idPartner)
        {
            return Message.GetChatWithPartner(idIntern, idPartner);
        }

        // GET: api/<Messages>
        [HttpPost]
        [Route("AddNewMessage")]
        public bool GetChatWithPartner([FromBody] Message m)
        {
            return m.AddNewMessage();
        }


    }
}
