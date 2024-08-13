using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using InternViewServer.Models;


namespace InternViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly ILoggerFactory _loggerFactory;

        public AlgorithmController(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        [HttpPut("GetOptimalAssignments/{startDate}/{endDate}")]
        public ActionResult<IEnumerable<OptimalAssignment>> GetOptimalAssignments(string startDate, string endDate)
        {
            // Create a logger specifically for the Algorithm class
            var algorithmLogger = _loggerFactory.CreateLogger<Algorithm>();
            var algorithm = new Algorithm(algorithmLogger);

            var assignments = algorithm.CalculateOptimalAssignments(startDate, endDate);
            return Ok(assignments);
        }

        // GET: api/<Algorithm_Weights>
        [HttpGet]
        [Route("Get_All_Algorithm_Weights")]
        public Algorithm_Weights Get_All_Algorithm_Weights()
        {
            return Algorithm_Weights.Read_Algorithm_Weights();
        }

        // PUT api/
        [HttpPut("updateAlgorithmWeights")]
        public bool UpdateAlgoWeights([FromBody] Algorithm_Weights weights)
        {
            return (weights.Update_Algorithm_Weights() == 1);
        }
    }
}
