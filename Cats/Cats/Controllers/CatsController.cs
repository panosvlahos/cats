using Hangfire;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Cats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CatsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST /api/cats/fetch
        [HttpPost("fetch")]
        public IActionResult Fetch()
        {
            var jobId = Hangfire.BackgroundJob.Enqueue<FetchCatsJob>(job => job.ExecuteAsync());
            return Ok(new { jobId });
        }

        // GET /api/cats/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatById(string id)
        {
            var cat = await _unitOfWork.CatRepository.GetCatByIdAsync(id);
            if (cat == null)
                return NotFound();

            return Ok(cat);
        }

        // GET /api/cats?page=1&pageSize=10
        // GET /api/cats?tag=playful&page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetCatsByTag([FromQuery] string? tag)
        {
            

            var cats = await _unitOfWork.CatRepository.GetCatsAsync(tag, page, pageSize);
            return Ok(cats);
        }

        [HttpGet]
        public async Task<IActionResult> GetCats([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1) return BadRequest("Page and PageSize must be greater than 0.");

            var cats = await _unitOfWork.CatRepository.GetCatsAsync(tag, page, pageSize);
            return Ok(cats);
        }
    }
}
