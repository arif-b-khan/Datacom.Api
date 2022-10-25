using AutoMapper;
using Payroll.Domain.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Models
{
    public class PayslipRequestProfile : Profile
    {
        public PayslipRequestProfile()
        {
            CreateMap<PayslipRequest, SalaryInfo>()
                .ForMember(s => s.Month, p => p.MapFrom(p1 => p1.PayPeriod));
        }
    }
}
