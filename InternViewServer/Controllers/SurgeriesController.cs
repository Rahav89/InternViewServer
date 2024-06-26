﻿using InternViewServer.Models;
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
        public IEnumerable<Surgeries> Get()
        {
            return Models.Surgeries.Read();
        }


        [HttpGet]
        [Route("GetSurgeriesByID/{surgeryID}")]//Use Resource routing
        public List<Surgeries> GetSurgeriesByID(int surgeryID)
        {
            return Surgeries.GetSurgeriesByID(surgeryID);//return null if doesnt found
        }

        [HttpGet]
        [Route("GetFutureSurgeries")]//Use Resource routing
        public List<Dictionary<string, object>> GetFutureSurgeries()
        {
            return Surgeries.GetFutureSurgeries();
        }

        [HttpGet]
        [Route("GetSurgeryRoles")]//Use Resource routing
        public List<Dictionary<string, object>> GetSurgeryRoles(int surgery_id)
        {
            return Surgeries.GetSurgeryRoles(surgery_id);
        }

        // GET api/<SurgeriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SurgeriesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SurgeriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SurgeriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
