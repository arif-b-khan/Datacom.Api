using NUnit.Framework;
using Payroll.Core.Engine;
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

        [SetUp]
        public void SetUp()
        {
            _ruleEngine = new RuleEngine("tax-rules.json");
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
    }
}
