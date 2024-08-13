using InternViewServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurgeriesController : ControllerBase
    {
        // GET: api/<SurgeriesController>
        [HttpGet]
        [Route("GetAllSurgeriesWithProcedures/")]

        public List<Dictionary<string, object>> GetAllSurgeriesWithProcedures()
        {
            return Models.Surgeries.GetAllSurgeriesWithProcedures();
        }

        // GET: api/<SurgeriesController>
        [HttpGet]
        [Route("AllSurgeriesWithInterns/")]

        public List<Dictionary<string, object>> AllSurgeriesWithInterns()
        {
            return Models.Surgeries.AllSurgeriesWithInterns();
        }

        [HttpGet]
        [Route("GetSurgeriesByID/{surgeryID}")]//Use Resource routing
        public List<Surgeries> GetSurgeriesByID(int surgeryID)
        {
            return Surgeries.GetSurgeriesByID(surgeryID);//return null if doesnt found
        }

        //[HttpGet]
        //[Route("GetAllSurgeries/")]
        //public List<Surgeries> GetAllSurgeries()
        //{
        //    return Models.Surgeries.GetAllSurgeries();
        //}
        //[HttpGet]
        //[Route("GetFutureSurgeries")]//Use Resource routing
        //public List<Dictionary<string, object>> GetFutureSurgeries()
        //{
        //    return Surgeries.GetFutureSurgeries();
        //}

        [HttpGet]
        [Route("GetSurgeryRoles")]//Use Resource routing
        public List<Dictionary<string, object>> GetSurgeryRoles(int surgery_id)
        {
            return Surgeries.GetSurgeryRoles(surgery_id);
        }

        //FOR ALGO :
        [HttpGet]
        [Route("GetSurgeriesByTime/{startDate}/{endDate}")]//Use Resource routing
        public List<Surgeries> GetSurgeriesByTime(string startDate, string endDate)
        {
            return Surgeries.GetSurgeriesByTime(startDate , endDate);//return null if doesnt found
        }

        [HttpGet]
        [Route("GetSurgeriesByTime/{SurgeryId}")]//Use Resource routing
        public List<int> GetProceduresOfSurgery(int SurgeryId)
        {
            return Surgeries.GetProceduresOfSurgery(SurgeryId);//return null if doesnt found
        }

        // GET api/<SurgeriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SurgeriesController>
        [HttpPost]
        [Route("AddSurgery")]//Use Resource routing
        public int Post([FromBody] Surgeries s)
        {
            return s.AddSurgery();
        }

        //!!for Procedure In Surgery
        // POST api/<SurgeriesController>
        [HttpPost]
        [Route("AddProcedureInSurgery/{surgery_id}/{procedure_Id}")]//Use Resource routing
        public bool AddProcedureInSurgery(int surgery_id, int procedure_Id)
        {
            return Surgeries.AddProcedureInSurgery(surgery_id , procedure_Id) ;
        }
        //!!for Procedure In Surgery

        // PUT api/<SurgeriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpPut("UpdateSurgeries/{id}")]
        public bool put(int id, [FromBody] Surgeries surgeries)
        {
            surgeries.Surgery_id = id;
            return (surgeries.UpdateSurgeries() == 1);
        } 

        // DELETE api/<SurgeriesController>/5
        [HttpDelete("DeleteSurgeryFromSurgeriesSchedule/{id}")]
        public int DeleteSurgeryFromSurgeriesSchedule(int id)
        {
            return Surgeries.DeleteSurgeryFromSurgeriesSchedule(id);
        }
    }
}
