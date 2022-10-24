using Payroll.Domain.Validators;
using Payroll.Models;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Domain.Models
{
    public record PayslipRequest
    {
        [Required(ErrorMessage = "Invlaid input. Provide value for FirstName")]
        [StringLength(100)] 
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Invlaid input. Provide value for LastName")]
        [StringLength(100)]
        public string LastName { get; set; }

        [MinimumValue(0, ErrorMessage = $"{nameof(AnnualSalary)} cannot be less than \"0\"")]
        public decimal AnnualSalary { get; set; }

        [MinimumValue(0, ErrorMessage = $"{nameof(AnnualSalary)} cannot be less than \"0\"")]
        public float SuperRate { get; set; }

        [EnumDataType(typeof(Months), ErrorMessage = "Invlaid input. Provide value for PayPeriod")]
        public Months PayPeriod { get; set; }

    }
}
