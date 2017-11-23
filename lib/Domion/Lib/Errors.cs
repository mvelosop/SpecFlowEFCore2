using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domion.Lib
{
    public static class Errors
    {
        public static List<ValidationResult> NoError { get; } = new List<ValidationResult>();

        public static ValidationResult Error(string message, params object[] values)
        {
            return new ValidationResult(string.Format(message, values));
        }

        public static ValidationResult Error(string message, object[] values, string[] properties)
        {
            return new ValidationResult(string.Format(message, values));
        }

        public static List<ValidationResult> ErrorList(string message, params object[] values)
        {
            return new List<ValidationResult> { Error(message, values) };
        }

        public static List<ValidationResult> ErrorList(string message, object[] values, string[] properties)
        {
            return new List<ValidationResult> { Error(message, values) };
        }
    }
}
