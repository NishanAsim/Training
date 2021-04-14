using System;

namespace Nishan.Data
{
    public class InvalidDatabaseConfigurationException : Exception
    {
        public InvalidDatabaseConfigurationException(string errorMessage):base(errorMessage)
        {
        }
    }
}