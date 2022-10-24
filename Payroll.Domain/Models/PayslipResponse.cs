namespace Payroll.Domain.Models
{
    public record PayslipResponse
    {
        public string? Name { get; set; }
        public string? PayPeriod { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal NetIncome { get; set; }
        public decimal Super { get; set; }
    }
}
