using System.Text.RegularExpressions;

using Norma.Eta.Properties;

namespace Norma.Eta.Validations
{
    public class RegexValidator : IValidator<string>
    {
        #region Implementation of IValidator<string>

        public string Validate(string value)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Regex.IsMatch("", value);
                return null;
            }
            catch
            {
                return Resources.InvalidRegex;
            }
        }

        public string Convert(string value) => value;

        #endregion
    }
}