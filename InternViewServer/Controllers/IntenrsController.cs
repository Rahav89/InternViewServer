using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntenrsController : ControllerBase
    {
        // GET: api/<Intenrs>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Intenrs>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Intenrs>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Intenrs>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Intenrs>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
