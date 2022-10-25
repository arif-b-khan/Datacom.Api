using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Payroll.Core.Engine;
using Payroll.Domain.AppSetting;
using Payroll.Domain.Salary;
using Payroll.Models;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Core.Tests
{
    [TestFixture]
    public class RuleEngineTest
    {
        private RuleEngine? _ruleEngine;
        private Lazy<Mock<IOptions<TaxSettingOptions>>> taxSettingsLazy = new Lazy<Mock<IOptions<TaxSettingOptions>>>(() =>
        {
            return new Mock<IOptions<TaxSettingOptions>>();
        }, true);

        private Mock<IOptions<TaxSettingOptions>> MockTaxSettingOptions { get { return taxSettingsLazy.Value; } }

        [SetUp]
        public void SetUp()
        {

            MockTaxSettingOptions.Setup(s => s.Value).Returns(() => new TaxSettingOptions()
            {
                InputName = "salary",
                WorkflowName = "TaxDeductionWorkflow", 
                RuleFileName= "tax-rules.json"
            });
            _ruleEngine = new RuleEngine(MockTaxSettingOptions.Object);
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public async Task ExecuteRulesAsyncTest()
        {
            dynamic taxObj = new ExpandoObject();
            taxObj.annualincome = 1000;
            var ruleParam = new RuleParameter("salary", taxObj);

            var ruleResults = await _ruleEngine.ExecuteRulesAsync("TaxDeductionWorkflow", ruleParam);
            Assert.IsNotNull(ruleResults);
            Assert.GreaterOrEqual(ruleResults.Count, 1);
        }

        private static IEnumerable<SalaryInfo> GetSalaryList() => new List<SalaryInfo>()
        {
            new SalaryInfo()
            {
                FirstName = "Arif",
                LastName = "Khan",
                AnnualSalary = 60050,
                SuperRate = 9,
                Month = Models.Months.March
            }
        };

        [TestCase("A1", "", 20000.00, 9, Months.March)]
        [TestCase("A2", "", 13000.00, 9, Months.March)]
        [TestCase("A3", "", 47000.00, 9, Months.March)]
        [TestCase("A4", "", 48500.00, 9, Months.March)]
        [TestCase("A5", "", 69000.00, 9, Months.March)]
        [TestCase("A6", "", 71000.00, 9, Months.March)]
        [TestCase("A7", "", 179000.00, 9, Months.March)]
        [TestCase("A8", "", 181000.00, 9, Months.March)]
        public async Task ExecuteRulesAsyncValidateResult(string firstname, string lastname, decimal annualsalary, float superrate, Months month)
        {
            var taxProcessor = new TaxProcessor(_ruleEngine, MockTaxSettingOptions.Object);
            var salaryInfo = new SalaryInfo()
            {
                FirstName = firstname,
                LastName = lastname,
                AnnualSalary = annualsalary,
                SuperRate = superrate,
                Month = month
            };

            var salaryResult = await taxProcessor.ProcessAsync(salaryInfo);
            Assert.IsNotNull(salaryResult);
            if (salaryResult.FirstName == "A1")
            {
                Assert.AreEqual(210.00, salaryResult.IncomeTax);
            }

            if (salaryResult.FirstName == "A2")
            {
                Assert.AreEqual(113.75, salaryResult.IncomeTax);
            }

            if (salaryResult.FirstName == "A3")
            {
                Assert.AreEqual(603.75, salaryResult.IncomeTax);
            }

            if (salaryResult.FirstName == "A4")
            {
                Assert.AreEqual(630.83, salaryResult.IncomeTax);
            }

            if (salaryResult.FirstName == "A5")
            {
                Assert.AreEqual(1143.33, salaryResult.IncomeTax);
            }

            if (salaryResult.FirstName == "A6")
            {
                Assert.AreEqual(1195.83, salaryResult.IncomeTax);
            }

            if (salaryResult.FirstName == "A7")
            {
                Assert.AreEqual(4165.83, salaryResult.IncomeTax);
            }

            if (salaryResult.FirstName == "A8")
            {
                Assert.AreEqual(4225.83, salaryResult.IncomeTax);
            }

        }
    }
}
