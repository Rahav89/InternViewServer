using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


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

        [HttpGet("GetOptimalAssignments/{startDate}/{endDate}")]
        public ActionResult<IEnumerable<OptimalAssignment>> GetOptimalAssignments(string startDate, string endDate)
        {
            // Create a logger specifically for the Algorithm class
            var algorithmLogger = _loggerFactory.CreateLogger<Algorithm>();
            var algorithm = new Algorithm(algorithmLogger);

            var assignments = algorithm.CalculateOptimalAssignments(startDate, endDate);
            return Ok(assignments);
        }
    }
}
