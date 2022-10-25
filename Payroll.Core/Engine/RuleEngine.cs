using Microsoft.Extensions.Options;
using Payroll.Domain.AppSetting;
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
        private string? _ruleFile;
        private RulesEngine.RulesEngine _ruleEngine;

        public RuleEngine(IOptions<TaxSettingOptions> taxSettings)
        {
            _ruleFile = taxSettings.Value.RuleFileName ?? throw new ArgumentNullException(nameof(taxSettings.Value.RuleFileName));
            _ruleEngine = CreateRuleEngine().GetAwaiter().GetResult();
        }

        protected virtual async Task<RulesEngine.RulesEngine> CreateRuleEngine()
        {
            try
            {
                var executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var workflowStr = await File.ReadAllTextAsync(Path.GetFullPath(Path.Combine(executingDir, "Rules", _ruleFile)));
                var serializationOptions = new System.Text.Json.JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
                var workflowViaTextJson = System.Text.Json.JsonSerializer.Deserialize<Workflow[]>(workflowStr, serializationOptions);
                return new RulesEngine.RulesEngine(workflowViaTextJson, new ReSettings());
            }
            catch (FileNotFoundException fileNFEx)
            {
                System.Diagnostics.Debug.WriteLine($"Message: {fileNFEx.Message}");
                throw;
            }
        }

        public async ValueTask<List<RuleResultTree>> ExecuteRulesAsync(string workflowName, params RuleParameter[] ruleParam)
        {
            return await _ruleEngine.ExecuteAllRulesAsync(workflowName, ruleParam);
        }
    }
}
