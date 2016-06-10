namespace Norma.Eta.Validations
{
    public interface IValidator<out T>
    {
        string Validate(string value);

        T Convert(string value);
    }
}