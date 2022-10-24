using RulesEngine.Models;

namespace Payroll.Core.Engine
{
    public interface IRuleEngine
    {
        ValueTask<List<RuleResultTree>> ExecuteRulesAsync(string ruleName, params RuleParameter[] ruleParam);
    }
}