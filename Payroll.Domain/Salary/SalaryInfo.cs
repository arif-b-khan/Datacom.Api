using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Salary
{
    public class SalaryInfo : ICloneable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public decimal AnnualSalary { get; set; }
        public float SuperRate { get; set; }
        public Months Month { get; set; }
        public string? PayPeriod { get; set; }
        public decimal GrossIncome { 
            get
            {
                if((int)Month != 0)
                {
                    return AnnualSalary / (int)Month;
                }
                return 0;
            }
        }
        private decimal _incomeTax;
        public decimal IncomeTax { get
            { 
            return _incomeTax;
            }set
            {
                NetIncome = GrossIncome - value;
                _incomeTax = value;
            }
        }
        public decimal NetIncome { get; set; }
        public decimal Super { get
            {
                decimal super = GrossIncome / Convert.ToDecimal(SuperRate);
                return Math.Round(super, 2);
            }
        }

        public object Clone()
        {
            return new SalaryInfo()
            {
                FirstName = FirstName,
                LastName = LastName,
                AnnualSalary = AnnualSalary,
                SuperRate = SuperRate,
                Month = Month,
                PayPeriod = PayPeriod,
                IncomeTax = IncomeTax,
                NetIncome = NetIncome
            };
        }
    }
}
