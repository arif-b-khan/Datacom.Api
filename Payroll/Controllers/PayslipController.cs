using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Domain.Models;
using Payroll.Models;

namespace Payroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayslipController : ControllerBase
    {
        private readonly ILogger<PayslipController> _logger;
        public PayslipController(ILogger<PayslipController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]List<PayslipRequest> request)
        {
            _logger.LogInformation(request.ToString());
            return Ok(request);
        }
    }
}
