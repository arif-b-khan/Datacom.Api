using Payroll.Domain.Salary;

namespace Payroll.Core
{
    public interface ITaxProcessor
    {
        Task<SalaryInfo> ProcessAsync(SalaryInfo salary);
    }
}