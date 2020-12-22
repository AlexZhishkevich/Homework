namespace Homework.TemperatureConverter.Validation
{
    public class CelsiusTemperatureValidation : IValidation<float?>
    {
        private const double MinPossibleCelsiusValue = -273.15d;

        public ValidationResult ValidateValue(float? parameter)
        {
            var validationErrorMessage = string.Empty;

            if (parameter == null)
            {
                validationErrorMessage = "Value can not be null";
                return new ValidationResult(false, validationErrorMessage);
            }

            if (parameter <= MinPossibleCelsiusValue)
            {
                validationErrorMessage = "Value can not be less than -273.15";
            }
            else if (parameter > double.MaxValue)
            {
                validationErrorMessage = $"Can not convert values more than {double.MaxValue}";
            }

            return new ValidationResult(string.IsNullOrEmpty(validationErrorMessage), validationErrorMessage);
        }
    }
}
