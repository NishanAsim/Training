using System;
using System.Runtime.Serialization;

namespace Nishan.Data
{
    public class SqlQueryArgumentException : ArgumentException
    {
        public SqlCommandErrorType ErrorType { get; set; } = SqlCommandErrorType.NotSet;
        public string ColumnName { get; set; } = string.Empty;
        public SqlQueryArgumentException()
        {
        }

        public SqlQueryArgumentException(string message) : base(message)
        {
        }

        public SqlQueryArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SqlQueryArgumentException(string message, string paramName) : base(message, paramName)
        {
        }

        public SqlQueryArgumentException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }

        protected SqlQueryArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static SqlQueryArgumentException GetSqlQueryArgumentException(string errorCode, string errorMessage, Exception innerException)
        {
            return new SqlQueryArgumentException(errorMessage, errorCode, innerException);
        }

        public static SqlQueryArgumentException GetSqlQueryArgumentException(string errorCode,
                                                                             string errorMessage,
                                                                             string columnName,
                                                                             SqlCommandErrorType errorType,
                                                                             Exception innerException)
        {
            return new SqlQueryArgumentException(errorMessage, errorCode, innerException)
            {
                 ErrorType = errorType,
                 ColumnName = columnName
            };
        }

    }
}