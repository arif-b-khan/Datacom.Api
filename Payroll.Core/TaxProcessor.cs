using Microsoft.Extensions.Options;
using Payroll.Core.Engine;
using Payroll.Domain.AppSetting;
using Payroll.Domain.Salary;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Payroll.Core.Tests")]
namespace Payroll.Core
{
    internal class TaxProcessor : ITaxProcessor
    {
        private readonly IRuleEngine? _ruleEngine;
        private readonly TaxSettingOptions _taxSettings;
        public TaxProcessor(IRuleEngine? ruleEngine, IOptions<TaxSettingOptions> taxSettings)
        {
            _ruleEngine = ruleEngine ?? throw new ArgumentNullException(nameof(ruleEngine));
            _taxSettings = taxSettings.Value ?? throw new ArgumentNullException(nameof(taxSettings));
        }

        public async Task<SalaryInfo> Process(SalaryInfo salary)
        {
            dynamic inputObj = new ExpandoObject();
            inputObj.annualsalary = salary.AnnualSalary;
            var ruleParam = new RuleParameter(_taxSettings.InputName, inputObj);
            var workflowResult = await _ruleEngine.ExecuteRulesAsync(_taxSettings.WorkflowName, ruleParam);
            var output = (SalaryInfo)salary.Clone();
            output.IncomeTax = GetIncomeTax(workflowResult, (int)salary.Month);
            return output;
        }

        private decimal GetIncomeTax(List<RuleResultTree> workflowResult, int month)
        {
            if(workflowResult == null)
                throw new ArgumentNullException(nameof(workflowResult));    

            decimal taxResult = 0m;
            foreach (var result in workflowResult)
            {
                Console.WriteLine($"Rule name: {result.Rule.RuleName}, IsSuccess: {result.IsSuccess}");
                foreach (var childResult in result.ChildResults)
                {
                    Console.WriteLine($"Rule name: {childResult.Rule.RuleName}, IsSuccess: {childResult.IsSuccess}" +
                        $", Output: {childResult.ActionResult.Output}");
                    taxResult += Convert.ToDecimal(childResult.ActionResult.Output);
                }
            }
            return Math.Round(taxResult / month, 2);
        }
    }
}
