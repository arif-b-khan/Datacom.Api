using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Core;
using Payroll.Domain.Models;
using Payroll.Domain.Salary;
using Payroll.Models;

namespace Payroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayslipController : ControllerBase
    {
        private readonly ILogger<PayslipController> _logger;
        private readonly ITaxProcessor _taxProcessor;
        private readonly IMapper _mapper;
        public PayslipController(ITaxProcessor taxProcessor, IMapper mapper, ILogger<PayslipController> logger)
        {
            _taxProcessor = taxProcessor;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<PayslipResponse>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] List<PayslipRequest> request)
        {

            var salaryInfoList = _mapper.Map<List<SalaryInfo>>(request);
            var salaryResponseList = new List<PayslipResponse>();
            foreach (var sal in salaryInfoList)
            {
                SalaryInfo salaryResponse = await _taxProcessor.ProcessAsync(sal);
                PayslipResponse payResponse = _mapper.Map<PayslipResponse>(salaryResponse);
                salaryResponseList.Add(payResponse);
                _logger.LogInformation(payResponse.ToString());
            }
            return Ok(salaryResponseList);
        }
    }
}
