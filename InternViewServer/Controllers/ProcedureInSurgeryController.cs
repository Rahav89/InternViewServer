using InternViewServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureInSurgeryController : ControllerBase
    {
        // GET: api/<ProcedureInSurgeryController>
        [HttpGet]
        public IEnumerable<ProcedureInSurgery> Get()
        {
            return Models.ProcedureInSurgery.Read();
        }

        // GET api/<ProcedureInSurgeryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProcedureInSurgeryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProcedureInSurgeryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProcedureInSurgeryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
