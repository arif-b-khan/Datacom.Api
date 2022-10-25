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
    public class TaxProcessor : ITaxProcessor
    {
        private readonly IRuleEngine? _ruleEngine;
        private readonly TaxSettingOptions _taxSettings;

        public TaxProcessor(IRuleEngine? ruleEngine, IOptions<TaxSettingOptions> taxSettings)
        {
            _ruleEngine = ruleEngine ?? throw new ArgumentNullException(nameof(ruleEngine));
            _taxSettings = taxSettings.Value ?? throw new ArgumentNullException(nameof(taxSettings));
        }

        public async Task<SalaryInfo> ProcessAsync(SalaryInfo salary)
        {
            dynamic inputObj = new ExpandoObject();
            inputObj.annualsalary = salary.AnnualSalary;
            var ruleParam = new RuleParameter(_taxSettings.InputName, inputObj);
            var workflowResult = await _ruleEngine.ExecuteRulesAsync(_taxSettings.WorkflowName, ruleParam);
            var output = (SalaryInfo)salary.Clone();
            output.IncomeTax = GetIncomeTax(workflowResult);
            return output;
        }

        /// <summary>
        /// Calculates the income tax
        /// </summary>
        /// <param name="workflowResult"><seealso cref="List{RuleResultTree}<"/> list of rule result</param>
        /// <returns>Income tax for e.g 101.10</returns>
        /// <exception cref="ArgumentNullException">Workflow cannot be null</exception>
        private decimal GetIncomeTax(List<RuleResultTree> workflowResult)
        {
            if(workflowResult == null)
                throw new ArgumentNullException(nameof(workflowResult));    

            decimal taxResult = 0m;
            foreach (var result in workflowResult)
            {
                Console.WriteLine($"Rule name: {result.Rule.RuleName}, IsSuccess: {result.IsSuccess}");
                if(result.ChildResults != null)
                {
                    foreach (var childResult in result.ChildResults)
                    {
                        Console.WriteLine($"Rule name: {childResult.Rule.RuleName}, IsSuccess: {childResult.IsSuccess}" +
                            $", Output: {childResult.ActionResult.Output}");
                        taxResult += Convert.ToDecimal(childResult.ActionResult.Output);
                    }
                }
            }
            return Math.Round(taxResult / 12, 2);
        }
    }
}
