using System.ComponentModel.DataAnnotations;

namespace DU_Summer_2023_Capstone.Data.Validation
{

    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string otherProperty;
        private readonly object value;

        public RequiredIfAttribute(string otherProperty, object value)
        {
            this.otherProperty = otherProperty;
            this.value = value;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(otherProperty);
            if (property == null)
                throw new ArgumentException($"Property {otherProperty} not found");

            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance);
            if (otherPropertyValue.Equals(value))
            {
                if (value == null || (value is string strValue && string.IsNullOrWhiteSpace(strValue)))
                    return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }


}
