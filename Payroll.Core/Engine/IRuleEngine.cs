using RulesEngine.Models;

namespace Payroll.Core.Engine
{
    internal interface IRuleEngine
    {
        ValueTask<List<RuleResultTree>> ExecuteRulesAsync(string ruleName, params RuleParameter[] ruleParam);
    }
}