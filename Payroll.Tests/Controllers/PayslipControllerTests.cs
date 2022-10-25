using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payroll.Controllers;
using Payroll.Core;
using Payroll.Domain.Models;
using Payroll.Domain.Salary;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Tests.Controllers
{
    [TestFixture]
    public class PayslipControllerTests
    {

        private IMapper _mapper;  
        private Mock<ILogger<PayslipController>> MockILoggerPaySlip 
        {
            get;
        }

        private Mock<ITaxProcessor> MockTaxProcessor { get; }
        public PayslipControllerTests()
        {
            MockILoggerPaySlip = new Mock<ILogger<PayslipController>>();
            MockTaxProcessor = new Mock<ITaxProcessor>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new[]
                {
                    "Payroll.Domain"
                });
            }).CreateMapper();
        }

        [SetUp]
        public void Initialize()
        {
            //MockILoggerPaySlip.Setup(i => i.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()))
            //    .Callback(() => { });
            MockTaxProcessor.Setup(t => t.ProcessAsync(It.IsAny<SalaryInfo>()))
                .Returns(() =>
                {
                    return Task.FromResult(new SalaryInfo() { 
                    FirstName = "Arif",
                    LastName = "Khan", 
                    AnnualSalary = 60050,
                    PayPeriod = "1 March - 31 March",
                    IncomeTax = 919.58m,
                    SuperRate = 450.38f,
                    Month = Months.September
                    });
                });
        }

        [Test]
        public async Task Post_SalaryInfo_Validation()
        {
            //Arrange
            var payslipController = new PayslipController(MockTaxProcessor.Object, _mapper, MockILoggerPaySlip.Object);
            //Act
            IActionResult actualResult = await payslipController.Post(new List<PayslipRequest>()
            {
                new PayslipRequest()
                {
                    FirstName = "Arif",
                    LastName = "Khan",
                    AnnualSalary = 60050,
                    PayPeriod=(Months)9,
                    SuperRate=9
                }
            });
            //Assert
            var result = actualResult as OkObjectResult;
            var salaryInfoListResult = result.Value as List<PayslipResponse>;
            Assert.IsNotNull(salaryInfoListResult);
            Assert.AreEqual(salaryInfoListResult.Count, 1);

        }
    }
}
