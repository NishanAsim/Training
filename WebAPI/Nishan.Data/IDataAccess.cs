using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Nishan.Data
{
    /// <summary>
    /// Represents low level database operations
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// DBMS type 
        /// </summary>
        /// <value></value>
        DBMSType DBMS { get; }
        /// <summary>
        /// DBMS version
        /// </summary>
        /// <value></value>
        long DBMSVersion { get; }
        /// <summary>
        /// SQL user login
        /// </summary>
        /// <value></value>
        string UserName { get; }
        /// <summary>
        /// Database Name
        /// </summary>
        /// <value></value>
        string Database { get; }
        /// <summary>
        /// Server url
        /// </summary>
        /// <value></value>
        string URL { get; }
        /// <summary>
        /// DBMS provider
        /// </summary>
        /// <value></value>
        string Provider { get; }
        /// <summary>
        /// Name of DBMS null function
        /// </summary>
        /// <value></value>
        string DbmsNullFunctionName { get; }
        /// <summary>
        /// Text representation of DBMS null value
        /// </summary>
        /// <value></value>
        string DbmsNullText { get; }
        /// <summary>
        /// DBMS string concatinator 
        /// </summary>
        /// <value></value>
        string StringConcatinator { get; }
        /// <summary>
        /// Default DateTime format
        /// </summary>
        /// <value></value>
        string DateFormat { get; }
        /// <summary>
        /// DateTime format
        /// </summary>
        /// <value></value>
        string DateTimeFormat { get; }
        /// <summary>
        /// Max function
        /// </summary>
        /// <value></value>
        string MaxFunction { get; }

        /// <summary>
        /// Executes a sql query asynchronously
        /// </summary>
        /// <param name="commandType">Type of command to be executed</param>
        /// <param name="query">SQL query to be executed</param>
        /// <param name="cancellationToken">Task Cancellation token</param>
        /// <returns>Awaitable task</returns>
        Task<DataSet> ExecuteCommandAsync(DBCmdType commandType, string query, CancellationToken cancellationToken = default);
        Task<DataSet> ExecuteRoutineAsync(string routineName, List<SqlCommandParameter> parameters, CancellationToken cancellationToken = default);
        string GetDateString(DateTime? Date);
        Task<IDataReader> GetReaderAsync(string SQL, DbConnection Connection, CommandBehavior commandBehaviour);
        Task<DateTime> GetServerDateAsync();
        Task<IDataReader> GetSingleRowAsync(string query, DbConnection activeConnection, CancellationToken cancellationToken);
        string MakeSQLString(string sourceQuery, bool considerBlankAsNull);
    }

}
