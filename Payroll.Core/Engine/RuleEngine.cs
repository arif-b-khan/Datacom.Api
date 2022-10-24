using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Payroll.Core.Engine
{
    public class RuleEngine : IRuleEngine
    {
        private string _ruleFile;
        private RulesEngine.RulesEngine _ruleEngine;

        public RuleEngine(string ruleFile)
        {
            _ruleFile = ruleFile ?? throw new ArgumentNullException(nameof(ruleFile));
            _ = CreateRuleEngine();
        }

        private async Task CreateRuleEngine()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var workflowStr = await File.ReadAllTextAsync(Path.GetFullPath(executingDir + $"//{_ruleFile}"));
            var serializationOptions = new System.Text.Json.JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
            var workflowViaTextJson = System.Text.Json.JsonSerializer.Deserialize<Workflow[]>(workflowStr, serializationOptions);
            _ruleEngine = new RulesEngine.RulesEngine(workflowViaTextJson, new ReSettings());
        }

        public async ValueTask<List<RuleResultTree>> ExecuteRulesAsync(string ruleName, params RuleParameter[] ruleParam)
        {
            return await _ruleEngine.ExecuteAllRulesAsync(ruleName, ruleParam);
        }
    }
}
