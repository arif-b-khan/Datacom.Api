using System.ComponentModel.DataAnnotations;

namespace Payroll.Domain.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)] 
    public class MinimumValueAttribute : ValidationAttribute
    {
        public int Minimum { get; private set; }
        public MinimumValueAttribute(int minimum)
        {
            Minimum = minimum;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            if(int.TryParse(value.ToString(), out int minimumValue))
            {
                if(Minimum >= minimumValue)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
