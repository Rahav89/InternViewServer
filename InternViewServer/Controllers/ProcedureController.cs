using InternViewServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureController : ControllerBase
    {
        // GET: api/<ProcedureController>
        [HttpGet]
        public IEnumerable<Procedure> GetAllProcedure()
        {
            return Procedure.Read();
        }

        [HttpGet]
        [Route("GetAllprocedureName/")]//Use Resource routing
        public List<Procedure> GetAllprocedureName( )
        {
            return Procedure.GetAllprocedureName();//return null if doesnt found
        }

        // GET api/<ProcedureController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProcedureController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProcedureController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProcedureController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
