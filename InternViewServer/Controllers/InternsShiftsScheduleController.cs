using Microsoft.AspNetCore.Mvc;
using InternViewServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternsShiftsScheduleController : ControllerBase
    {    

        [HttpGet]
        [Route("GetAllInternsDutySchedule")]
        public List<InternSchedule> GetAllInternsShiftsSchedule()
        {
            return InternSchedule.GetAllInternsShiftsSchedule();
        }

        [HttpGet]
        [Route("GetInternsOnDutyDayBefore/{GivenDate}")]
        public List<int> GetInternsOnDutyDayBefore(DateTime GivenDate)
        {
            return InternSchedule.GetInternsOnDutyDayBefore(GivenDate);
        }

        [HttpPost]
        [Route("AddInternDutySchedule")]//Use Resource routing 
        public int AddInternsShiftsSchedule([FromBody] InternSchedule IS)
        {
            return IS.AddInternsShiftsSchedule();
        }

        // PUT api/<InternsShiftsScheduleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InternsShiftsScheduleController>/5
        [HttpDelete]
        [Route("DeleteInternDutySchedule")]//Use Resource routing 
        public bool DeleteInternsShiftsSchedule([FromBody] InternSchedule IS)
        {
            return IS.DeleteInternsShiftsSchedule();
        }
    }
}
