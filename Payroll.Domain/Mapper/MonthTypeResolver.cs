using AutoMapper;
using Payroll.Domain.Models;
using Payroll.Domain.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Mapper
{
    public class MonthTypeResolver : IValueResolver<SalaryInfo, PayslipResponse, string>
    {
        public string Resolve(SalaryInfo source, PayslipResponse destination, string destMember, ResolutionContext context)
        {
            var date = new DateTime(DateTime.Now.Year, (int)source.Month, 1);
            int lastDay = new DateTime(date.Year, date.Month, date.Day).AddMonths(1).AddDays(-1).Day;
            return $"1 - {source.Month}, {lastDay} - {source.Month}";
        }
    }
}
