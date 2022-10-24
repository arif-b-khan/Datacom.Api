using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Payroll.Core.Engine;
using Payroll.Domain.AppSetting;
using Payroll.Domain.Salary;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Core.Tests.Engine
{
    [TestFixture]
    internal class TaxProcessorTest
    {
        private ITaxProcessor? Processor { get; set; }
        private Lazy<Mock<IRuleEngine>> ruleEngineLazy = new Lazy<Mock<IRuleEngine>>(() =>
        {
            return new Mock<IRuleEngine>();
        }, true);
        private Mock<IRuleEngine> MockRuleEngine { get { return ruleEngineLazy.Value; } }
        private Lazy<Mock<IOptions<TaxSettingOptions>>> taxSettingsLazy = new Lazy<Mock<IOptions<TaxSettingOptions>>>(() =>
        {
            return new Mock<IOptions<TaxSettingOptions>>();
        }, true);

        private Mock<IOptions<TaxSettingOptions>> MockTaxSettingOptions { get { return taxSettingsLazy.Value; } }

        [SetUp]
        public void Setup()
        {
            MockRuleEngine.Setup(c => c.ExecuteRulesAsync(It.IsAny<string>(), It.IsAny<RuleParameter[]>()))
                .Returns(() =>
                {
                    var resultSetTree = new ValueTask<List<RuleResultTree>>(new List<RuleResultTree>()
                    {
                        new RuleResultTree()
                        {
                            IsSuccess = true,
                            ActionResult = new ActionResult()
                            {
                                Output = "Test",
                                Exception = null
                            },
                            Rule = new Rule()
                            {
                                RuleName = "TestRule"
                            },
                            ChildResults = new List<RuleResultTree>()
                            {
                                    new RuleResultTree()
                                    {
                                        IsSuccess = true,
                                        ActionResult = new ActionResult()
                                        {
                                            Output = 909,
                                            Exception = null
                                        },
                                        Rule = new Rule()
                                        {
                                            RuleName = "TestChildRule"
                                        }
                                    }
                            }
                        }
                    });
                    return resultSetTree;
                });

            MockTaxSettingOptions.Setup(s => s.Value).Returns(() => new TaxSettingOptions()
            {
                InputName = "salary",
                WorkflowName = "TaxDeductionWorkflow"
            });

            Processor = new TaxProcessor(MockRuleEngine.Object, MockTaxSettingOptions.Object);
        }

        [Test]
        public async Task ProcessText()
        {
            //Arrange
            var salaryInfo = new SalaryInfo()
            {
                FirstName = "Arif",
                LastName = "Khan",
                AnnualSalary = 60050,
                SuperRate = 9,
                Month = Models.Months.March
            };

            //Act
            var salaryInfoActual = await Processor.Process(salaryInfo);
            //assert
            Assert.IsNotNull(salaryInfoActual);
            Assert.AreEqual(75.75, salaryInfoActual.IncomeTax);

        }

    }
}
