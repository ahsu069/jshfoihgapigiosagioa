using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace api.Commons
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates any object using DataAnnotation attributes.
        /// </summary>
        public static bool TryValidate<T>(T? obj, out List<ValidationResult> results)
        {
             results = new List<ValidationResult>();

            if (obj == null)
            {
                results.Add(new ValidationResult("Object to validate cannot be null"));
                return false;
            }

            var context = new ValidationContext(obj);
            return Validator.TryValidateObject(
                obj,
                context,
                results,
                validateAllProperties: true
            );
            // var context = new ValidationContext(obj);
            // results = new List<ValidationResult>();

            // return Validator.TryValidateObject(
            //     obj,
            //     context,
            //     results,
            //     validateAllProperties: true
            // );
        }

        /// <summary>
        /// Throws ValidationException if invalid.
        /// </summary>
        public static void EnsureValid<T>(T? obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object to validate cannot be null");

            var context = new ValidationContext(obj);
            Validator.ValidateObject(obj, context, validateAllProperties: true);
        }

        /// <summary>
        /// Returns a string summary of all validation errors.
        /// </summary>
        public static string GetErrorSummary(List<ValidationResult> results)
        {
            return string.Join("; ", results.Select(r => r.ErrorMessage));
        }
        public static Dictionary<string, string[]> GetErrorDictionary(List<ValidationResult> results)
        {
            return results
                .SelectMany(r => r.MemberNames.Select(m => new { m, r.ErrorMessage }))
                .GroupBy(x => x.m)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage ?? "").ToArray()
                );
        }
        public static bool TryValidateForeignKey<T>(
            T id,
            Func<T, bool> existsFunc,
            out string error,
            [CallerArgumentExpression("id")] string? paramName = null)
        {
            paramName = CleanParamName(paramName);

            if (IsNullOrDefault(id))
            {
                error = $"Foreign key '{paramName}' cannot be null or default.";
                return false;
            }

            if (!existsFunc(id))
            {
                error = $"Referenced '{paramName}' not found.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private static bool IsNullOrDefault<T>(T value)
        {
            if (value == null)
                return true;

            var type = typeof(T);

            // For Guid or numeric types, check default value
            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return value.Equals(defaultValue);
            }

            // For strings, check empty or whitespace
            if (value is string s)
                return string.IsNullOrWhiteSpace(s);

            return false;
        }

        private static string? CleanParamName(string? expr)
        {
            if (string.IsNullOrWhiteSpace(expr))
                return expr;

            // Take the last segment after '.' — e.g., "request.role_id" → "role_id"
            var parts = expr.Split('.');
            return parts[^1];
        }
    }
}