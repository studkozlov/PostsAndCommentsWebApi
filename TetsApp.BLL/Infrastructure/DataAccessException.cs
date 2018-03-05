using System;

namespace TestApp.BLL.Infrastructure
{
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException) : 
            base($"{message} (See inner exception message for details.)", innerException) { }
    }
}
