using Payroll.Domain.Salary;

namespace Payroll.Core
{
    internal interface ITaxProcessor
    {
        Task<SalaryInfo> Process(SalaryInfo salary);
    }
}