using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.Helpers
{
    internal class ValidFieldsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedFields;

        public ValidFieldsAttribute(params string[] allowedFields)
        {
            _allowedFields = allowedFields;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            var stringValue = value.ToString();

            return _allowedFields.Contains(stringValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Allowed values are: {string.Join(", ", _allowedFields)}";
        }
    }
}
