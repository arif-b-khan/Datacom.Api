using Payroll.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Core
{
    internal class TaxProcessor
    {
        private readonly IRuleEngine? _ruleEngine;
        
        public TaxProcessor(IRuleEngine? ruleEngine)
        {
            _ruleEngine = ruleEngine ?? throw new ArgumentNullException(nameof(ruleEngine));
        }

        public async Task<> Process(decimal annualsalary, float superRate, int month)
        {

        }

    }
}
