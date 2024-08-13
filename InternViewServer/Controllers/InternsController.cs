using InternViewServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternsController : ControllerBase
    {
        // GET: api/<Intenrs>
        [HttpGet]
        public IEnumerable<Intern> Get()
        {
            return Models.Intern.Read();
        }

        [HttpGet]
        [Route("AllInternSurgeries")]
        public List<Dictionary<string, object>> AllInternSurgeries(int internId)
        {
            return Models.Intern.AllInternSurgeries(internId);
        }


        //!!!!!!for InternInSurgery
        [HttpPut]
        [Route("UpdateOrAddInternInSurgery")]
        public bool UpdateOrAddInternInSurgery([FromBody] InternInSurgery match)
        {
            return Models.InternInSurgery.UpdateOrAddInternInSurgery(match);
        }
        //!!!!!!for InternInSurgery

        [HttpGet]
        [Route("FiveRecentInternSurgeries")]
        public IEnumerable<SurgeriesOfIntern> FiveRecentInternSurgeries(int internId)
        {
            return Models.Intern.FiveRecentInternSurgeries(internId);
        }

        [HttpGet]
        [Route("GetSyllabusOfIntern")]
        public IEnumerable<SyllabusOfIntern> GetSyllabusOfIntern(int internId)
        {
            return Models.Intern.GetSyllabusOfIntern(internId);
        }


        [HttpGet]
        [Route("GetInternProcedureCounter")]
        public IEnumerable<InternProcedureCounter> GetInternProcedureCounter()
        {
            return Models.Intern.InternProcedureSummary();
        }

        [HttpGet]
        [Route("fullDetailedSyllabusOfIntern")]
        public List<Dictionary<string, object>> FullDetailedSyllabusOfIntern(int internID)
        {
            return Models.Intern.FullDetailedSyllabusOfIntern(internID);
        }

        //!!!!!!for algo
        [HttpGet]
        [Route("GetInternSyllabusForAlgo")]
        public List<Dictionary<string, object>> GetInternSyllabusForAlgo(int internID)
        {
            return Models.Intern.GetInternSyllabusForAlgo(internID);
        }
        //!!!!!!!!!!for algo

        // GET api/<Intenrs>/5
        [HttpGet("GetInternByID/{id}")]
        public Intern GetInternByID(int id)
        {
            return Models.Intern.GetInternByID(id);
        }


        // GET api/<Intenrs>/5
        [HttpGet("checkEmailIntern/{email}")]
        public int checkEmailIntern(string email)
        {
            return Models.Intern.checkEmailIntern(email);
        }


        // POST api/<Intenrs>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Intenrs>/5
        [HttpPut("updateIntern/{id}")]
        public bool put(int id, [FromBody] Intern intern)
        {
            intern.Id = id;
            return (intern.UpdateIntern() == 1);
        }
        // PUT api/<Intenrs>/5
        [HttpPut("UpdateInternPassword/{email}/{password}")]
        public bool UpdateInternPassword(string email, string password)
        {
            return (Intern.UpdateInternPassword(email,password) == 1);
        }

        // DELETE api/<Intenrs>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("LogInIntern")] //use query string
        public Intern LogInUser([FromBody] Intern i)
        {
            return Models.Intern.LogIn(i.Id, i.Password_i);
        }

        [HttpPost]
        [Route("AddIntern")] //use query string
        public bool AddIntern([FromBody] Intern i)
        {
            return i.AddIntern();
        }

        [HttpGet]
        [Route("GetInternSurgeriesByProcedure/{procedureID}/{internId}")]//Use Resource routing
        public List<Dictionary<string, object>> GetInternSurgeriesByProcedure(int internId, int procedureID)
        {
            return Intern.GetInternSurgeriesByProcedure(internId, procedureID);
        }

        [HttpGet]
        [Route("GetInternSurgeriesByProcedureName/{procedureName}/{InternID}")]//Use Resource routing
        public List<Dictionary<string, object>> GetInternSurgeriesByProcedureName(int InternID, string procedureName)
        {
            return Intern.GetInternSurgeriesByProcedureName(InternID, procedureName);
        }

        // GET: api/<Messages>
        [HttpGet]
        [Route("GetInternsForChat")]
        public List<Dictionary<string, object>> GetInternsForChat(int id)
        {
            return Intern.GetInternsForChat(id);
        }
    }
}
