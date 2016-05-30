using System;

using Norma.Eta.Properties;

namespace Norma.Eta.Validations
{
    public class DateTimeValidator : IValidator<DateTime>
    {
        private readonly bool _time;

        public DateTimeValidator(bool isTimeNeeded = false)
        {
            _time = isTimeNeeded;
        }

        #region Implementation of IValidator

        public string Validate(string value)
        {
            DateTime dateTime;
            if (!DateTime.TryParse(value, out dateTime))
                return Resources.InvalidDateTime;
            if (_time && dateTime < DateTime.Now)
                return Resources.InvalidDateTime2;
            if (dateTime < DateTime.Today)
                return Resources.InvalidDateTime2;
            return null;
        }

        public DateTime Convert(string value) => DateTime.Parse(value);

        #endregion
    }
}