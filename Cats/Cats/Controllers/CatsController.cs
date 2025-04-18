using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Cats.Controllers
{
    [ApiController]
    [Route("api/cats")]
    public class CatsController: ControllerBase
    {
     

        [HttpPost("fetch")]
        public IActionResult Fetch()
        {
            var jobId = BackgroundJob.Enqueue<FetchCatsJob>(job => job.ExecuteAsync());
            return Ok(new { jobId });
        }

        //[HttpGet("Get")]
        //public IActionResult Fetch()
        //{
        //    var jobId = BackgroundJob.Enqueue<FetchCatsJob>(job => job.ExecuteAsync());
        //    return Ok(new { jobId });
        //}
    }
}
