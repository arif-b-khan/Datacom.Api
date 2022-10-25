using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.AppSetting
{
    public class TaxSettingOptions
    {
        public const string TaxSettings = "TaxSettings";
        public string? InputName { get; set; }
        public string? WorkflowName { get; set; }
        public string? RuleFileName { get; set; }
    }
}
