namespace FinDataWebAPI.Controllers
{
    using FinDataWebAPI.Services;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class FinDataController : ControllerBase
    {
        private readonly IFinDataService _finDataService;
        private readonly ILogger<FinDataController> _logger;
        public FinDataController(IFinDataService finDataService, ILogger<FinDataController> logger)
        {
            _finDataService = finDataService;
            _logger = logger;
        }

        [HttpGet("/finData")]
        public async Task<IActionResult> GetFinancialData([FromQuery] string company, [FromQuery] DateTime date)
        {
            _logger.LogInformation(company + "'s Data requested");
            try
            {
                var data = await _finDataService.GetFinancialDataAsync(company, date);
                if(data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("/news")]
        public async Task<IActionResult> GetLatestNews([FromQuery] string company)
        {
            _logger.LogInformation(company + "news requested");
            try
            {
                var data = await _finDataService.GetLatestNewsAsync(company);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }


}
