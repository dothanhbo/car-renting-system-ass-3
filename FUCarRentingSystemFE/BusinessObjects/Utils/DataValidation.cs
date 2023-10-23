using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DateRangeAttribute : ValidationAttribute
{
    private DateTime _minDate;
    private DateTime _maxDate;

    public DateRangeAttribute(string minDate, string maxDate)
    {
        _minDate = DateTime.ParseExact(minDate, "MM/dd/yyyy", null);
        _maxDate = DateTime.ParseExact(maxDate, "MM/dd/yyyy", null);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if(value == null)
            return ValidationResult.Success;
        if (value != null && value is DateTime dateValue)
        {
            if (dateValue >= _minDate && dateValue <= _maxDate)
            {
                return ValidationResult.Success;
            }
        }
        

        return new ValidationResult(ErrorMessage);
    }
}
