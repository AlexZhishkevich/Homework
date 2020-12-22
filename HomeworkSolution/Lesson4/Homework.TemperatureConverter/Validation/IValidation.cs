namespace Homework.TemperatureConverter.Validation
{
    public interface IValidation<T>
    {
        ValidationResult ValidateValue(T parameter);
    }
}
