using AutoMapper;
using Payroll.Domain.Mapper;
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
            CreateMap<SalaryInfo, PayslipResponse>()
                .ForMember(p => p.PayPeriod, m => m.MapFrom<MonthTypeResolver>())
                .ForMember(p => p.GrossIncome, m => m.MapFrom(m => Math.Round(m.GrossIncome, 2)))
                .ForMember(p => p.IncomeTax, m => m.MapFrom(m => Math.Round(m.IncomeTax, 2)))
                .ForMember(p => p.Super, m => m.MapFrom(m => Math.Round(m.Super, 2)))
                .ForMember(p => p.NetIncome, m => m.MapFrom(m => Math.Round(m.NetIncome, 2)));


        }
    }
}
