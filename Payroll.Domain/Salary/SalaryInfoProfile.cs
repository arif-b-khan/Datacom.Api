using AutoMapper;
using Payroll.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Salary
{
    public class SalaryInfoProfile : Profile
    {
        public SalaryInfoProfile()
        {
            CreateMap<SalaryInfo, PayslipResponse>();
        }
    }
}
