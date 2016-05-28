namespace Norma.Eta.Validations
{
    public interface IValidator<T>
    {
        string Validate(string value);

        T Convert(string value);
    }
}