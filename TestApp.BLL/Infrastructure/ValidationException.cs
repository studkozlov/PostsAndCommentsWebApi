using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApp.BLL.Infrastructure
{
    public class ValidationException : Exception
    {
        public IList<(string, string)> ValidationErrors { get; protected set; }
        public ValidationException(IList<(string, string)> errors)
        {
            ValidationErrors = errors;
        }
        public override string Message
        {
            get
            {
                var messages = ValidationErrors.Select(dp => $"Field: {dp.Item1}  Error: {dp.Item2}").ToList();
                return string.Join(";", messages);
            }
        }
    }
}
