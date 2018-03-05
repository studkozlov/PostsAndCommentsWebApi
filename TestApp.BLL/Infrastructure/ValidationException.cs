using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApp.BLL.Infrastructure
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string> ValidationErrors { get; protected set; }
        public ValidationException(IDictionary<string, string> errors)
        {
            ValidationErrors = errors;
        }
        public override string Message
        {
            get
            {
                var messages = ValidationErrors.Select(dp => $"Field: {dp.Key}  Error: {dp.Value}").ToList();
                return string.Join(";", messages);
            }
        }
    }
}
