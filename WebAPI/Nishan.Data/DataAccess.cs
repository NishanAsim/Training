using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nishan.Logger;

namespace Nishan.Data
{

    public class DataAccess : IDataAccess
    {

        #region Field Definition Block
        // DBMS Information

        // connection parameters
        private readonly string userName;
        private readonly string password;
        private readonly string databaseName;
        private readonly bool useVerboseLogging;
        private readonly string serverUrl;
        private readonly bool useUnicodeConverter;
        private readonly string requestId;

        protected string UniCodeConvertChar
        {
            get
            {
                return "N"; //Check DBMS Type if required
            }
        }

        #endregion
        #region Property Definition Block
        /// <summary>
        /// Type of DBMS
        /// </summary>
        public DBMSType DBMS { get; }
        /// <summary>
        /// DBMS Version
        /// </summary>
        public long DBMSVersion { get; }
        /// <summary>
        /// DBMS userName
        /// </summary>
        public string UserName => userName ?? string.Empty;


        // do not expose password to other assemblies
        /// <summary>
        /// Password
        /// </summary>
        internal string Password => password ?? string.Empty;

        /// <summary>
        /// Databasename
        /// </summary>
        public string Database => databaseName ?? string.Empty;

        /// <summary>
        /// Server URL
        /// </summary>
        public string URL => serverUrl ?? string.Empty;

        /// <summary>
        /// Provider
        /// </summary>
        public string Provider { get; }


        #endregion
        #region Other Utility Members

        /// <summary>
        /// DBMS specific null function name
        /// </summary>
        public string DbmsNullFunctionName { get; }

        /// <summary>
        /// DBMS specific string representation of null
        /// </summary>
        public string DbmsNullText { get; }

        /// <summary>
        /// Property for string concatinator in SQL ("+" in SQL Server , "||" in Oracle)
        /// </summary>
        public string StringConcatinator
        {
            get;
            //set { stringConcatinator = value; }
        }

        /// <summary>
        /// DBMS specific date format
        /// </summary>
        public string DateFormat { get; }

        /// <summary>
        /// DBMS specific datetime format
        /// </summary>
        public string DateTimeFormat { get; }

        /// <summary>
        /// DBMS specific max function name
        /// </summary>
        public string MaxFunction { get; }

        #endregion
        #region Constructor

        public ILogger Logger { get; }

        public string RequestId => requestId ?? string.Empty;

        public DataAccess(IConnectionConfiguration configuration, ILogger logger, string requestId) : this(DBMSType.DbMySQL, configuration.UserName, configuration.Password, configuration.Url, configuration.Database, configuration.UseVerboseLog)
        {
            Logger = logger;
            this.requestId = requestId;
            VerboseLog($"Data access object initialized", "DataAccess", new string[] { configuration.Url, configuration.Database, configuration.UserName });
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DBMS">Type of the DBMS</param>
        /// <param name="userName">User Name of the system</param>
        /// <param name="password">Password</param>
        /// <param name="URL">Url </param>
        /// <param name="database">Database name</param>
        internal DataAccess(DBMSType DBMS, string userName, string password, string URL, string database, bool useVerboseLogging = false)
        {
            this.DBMS = DBMS;
            this.userName = userName;
            this.password = password;
            serverUrl = URL;
            databaseName = database;
            this.useVerboseLogging = useVerboseLogging;
            useUnicodeConverter = true;
            // validate the connection parameters
            if (string.IsNullOrWhiteSpace(userName)
                || string.IsNullOrWhiteSpace(database)
                || string.IsNullOrWhiteSpace(URL))
            {
                //TODO: Enhance to throw specific error message
                throw new InvalidDatabaseConfigurationException("User name, database or server name is not set");
            }
            // DBMS independent parameters

            DbmsNullText = "NULL";
            MaxFunction = "MAX";
            //Provider=MSDAORA.1;Data Source=orcl9i;Persist Security Info=True;User ID=fa
            // set the parameters as per current DBMS             
            switch (DBMS)
            {
                case DBMSType.DbMySQL:
                    Provider = "MySqlSource";
                    DbmsNullFunctionName = "coalesce";
                    DateFormat = "yyyy-MM-dd";
                    DateTimeFormat = "yyyy-MM-dd hh:mm:ss.mmm";
                    StringConcatinator = "||";
                    break;
                default:
                    throw new InvalidDatabaseConfigurationException("Unknown database type");
            }
            DBMSVersion = 0;

            connectionStateChangeHandler = new StateChangeEventHandler(DatabaseConnectionStateChanged);
            objectDisposedHandler = new EventHandler(DisposedEventHandler);
        }
        #endregion

        protected virtual string GetConnectionString()
        {
            string connectionString = string.Empty;
            //todo: convert to switch statement
            if (DBMS == DBMSType.DbMsSQLSrv)
            {
                connectionString = "User Id=" + userName + ";pwd=" + Password + ";Initial Catalog=" + Database;
                connectionString += ";Persist Security Info=False";
                connectionString += "; Server=" + URL;
            }
            if (DBMS == DBMSType.DbOracle)
            {

                //If Database is oracle then set the oracle version
                //Start > Run > regedit > HKEY_LOCAL_MACHINE > SOFTWARE > MICROSOFT>MSDTC > MTxOCI
                //Change All DLL version in MTxOCI Registry Folder

                connectionString = "User Id=" + userName + ";Password=" + Password;
                connectionString += ";Data Source =" + URL + "; Unicode=True ";

                //dataProvider           = "OraOLEDB.Oracle.1";
                //dataProvider          = "MSDAORA.1";
                //strConnString += ";Persist Security Info=False;Provider=" + Provider;
                //strConnString += "; Data Source =" + URL;
            }
            if (DBMS == DBMSType.DbMySQL)
            {

                connectionString = "User Id=" + userName + ";Password=" + Password + ";Initial Catalog=" + Database;
                connectionString += ";Server=" + URL;
                connectionString += ";Port=3306"; //TODO: Check what to do for a different port


            }

            return connectionString;
        }

        private DbParameter GetDbParameter(SqlCommandParameter parameter)
        {
            //TODO: SHOULD BE BASED ON A SWITCH CASE FOR DIFFERENT DbmSs 

            DbParameter dbParam = new MySqlParameter(parameter.Name, parameter.Value)
            {
                Direction = GetSqlParameterDirection(parameter.ParameterDirection)
            };

            return dbParam;

        }

        private static ParameterDirection GetSqlParameterDirection(SqlParameterDirection parameterType) =>
            parameterType switch
            {
                SqlParameterDirection.In => ParameterDirection.Input,
                SqlParameterDirection.Out => ParameterDirection.Output,
                SqlParameterDirection.InOut => ParameterDirection.InputOutput,
                _ => throw new Exception("Parameter type not supported")
            };


        /// <summary>
        /// This method creates a new connection to the database using connection parameters
        /// </summary>
        internal async Task<DbConnection> GetNewConnection()
        {
            try
            {
                var connection = GetNewConnectionObject();
                await connection.OpenAsync();
                return connection;
            }
            catch (Exception ex)
            {
                Logger.LogError(RequestId, "Database connection - failed to open, see exception below for details");
                throw new DataException("Database connection -  failed to open", ex);
            }
            //return objConn;
        }

        private readonly StateChangeEventHandler connectionStateChangeHandler;
        private readonly EventHandler objectDisposedHandler;

        private void DisposedEventHandler(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (sender is DbConnection connection)
                {
                    VerboseLog("Connection is disposed");
                    connection.Disposed -= objectDisposedHandler;
                }

                if (sender is DbCommand command)
                {
                    VerboseLog("Command is disposed");
                    command.Disposed -= objectDisposedHandler;
                }
            }
        }

        private void DatabaseConnectionStateChanged(object sender, StateChangeEventArgs e)
        {
            VerboseLog($"Database connection state changed from {e.OriginalState} to {e.CurrentState}");
            if (e.CurrentState == ConnectionState.Closed)
            {
                if (sender is DbConnection connection)
                { // remove handler
                    connection.StateChange -= connectionStateChangeHandler;
                }
            }

        }


        //TODO: TO BE REVISITED

        /// <summary>
        /// Filter and non sql character (e.g. "'") to avoid sql syntax error  
        /// </summary>
        /// <param name="sourceQuery">Source string to check</param>
        /// <param name="considerBlankAsNull">if blank string to be replaced with Null </param>
        /// <returns>The prepared SQL string</returns>
        public string MakeSQLString(string sourceQuery, bool considerBlankAsNull)
        {
            string preparedQuery = "";
            if (string.IsNullOrWhiteSpace(sourceQuery))
            {
                preparedQuery = considerBlankAsNull ? string.Empty : "''";
            }
            else
            {// string contains characters
                int i, lastPosition;
                i = 0;
                while ((lastPosition = sourceQuery.IndexOf("'", i)) != -1)
                {
                    preparedQuery = $"{preparedQuery}{sourceQuery[i..(lastPosition + 1)]}'";
                    i = lastPosition + 1;
                }
                preparedQuery = $"{preparedQuery}{sourceQuery[i..]}";
                preparedQuery = (useUnicodeConverter ? UniCodeConvertChar : "") + $@"'{preparedQuery}'";
            }
            return preparedQuery;
        }

        /// <summary>
        /// Execute a SQL 
        /// </summary>
        /// <param name="commandType">Type of the command Select,Insert,Update or Delete</param>
        /// <param name="query">SQL command</param>
        /// <returns>Awaitable task containing data set</returns>
        public async Task<DataSet> ExecuteCommandAsync(DBCmdType commandType, string query, CancellationToken cancellationToken = default)
        {
            using var connection = await GetNewConnection();
            var dataSet = await ExecuteCommandAsync(commandType, query, null, connection, null);
            connection.Close();

            return dataSet;
        }

        private async Task<DataSet> ExecuteCommandAsync(DBCmdType commandType,
                                                               string query,
                                                               DbTransaction activeTransaction,
                                                               List<SqlCommandParameter> parameters = null
                                                               )
        {
            using var dbConnection = await GetNewConnection();
            var result = await ExecuteCommandAsync(commandType, query, activeTransaction, dbConnection, parameters);
            dbConnection.Close();
            return result;
        }

        private async Task<DataSet> ExecuteCommandAsync(DBCmdType commandType,
                                                        string query,
                                                        DbTransaction activeTransaction,
                                                        DbConnection dataConnection,
                                                        List<SqlCommandParameter> parameters = null,
                                                        CancellationToken cancellationToken = default
                                                        )
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Sql command can not be blank", nameof(query));
            }

            if (dataConnection is null)
            {
                throw new ArgumentNullException(nameof(dataConnection));
            }

            using var sqlCommand = GetNewCommandObject();
            sqlCommand.Connection = dataConnection;
            sqlCommand.CommandText = query;
            if (commandType == DBCmdType.DbCmdStoredProc)
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
            }

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    sqlCommand.Parameters.Add(GetDbParameter(parameter));
                }
            }

            //TODO : Need to test transactions
            if (activeTransaction != null)
            {
                sqlCommand.Transaction = activeTransaction;
            }

            VerboseLog($"Executing Command, {DumpCommandValues(sqlCommand)}");
            if (sqlCommand.Parameters.Count > 0)
            {
                // VerboseLog("List of parameters :");
                foreach (DbParameter sqlParameter in sqlCommand.Parameters)
                {
                    VerboseLog($"Command Parameter", "DataAccess", DumpSqlParameter(sqlParameter));
                }
            }
            else
            {
                VerboseLog("No query parameters present");
            }

            return await CommandExecutor.ExecuteDbTask(
                    async (taskCancellationToken) =>
                    {
                        DataSet result = null;
                        switch (commandType)
                        {
                            case DBCmdType.DbCmdSelect:
                            case DBCmdType.DbCmdStoredProc:
                                result = await GetResultSet(sqlCommand, taskCancellationToken);
                                if (parameters != null)
                                {
                                    foreach (var outParam in parameters.Where(p => IsOutputParameter(p)))
                                    {
                                        outParam.Value = sqlCommand.Parameters[outParam.Name].Value;
                                        VerboseLog($"Output parameter", "DataAccess", DumpSqlParameter(sqlCommand.Parameters[outParam.Name]));
                                    }
                                }
                                break;
                            case DBCmdType.DbCmdUpdate:
                            case DBCmdType.DbCmdInsert:
                            case DBCmdType.DbCmdDelete:
                                await sqlCommand.CheckAndExecuteNonQueryAsync(taskCancellationToken);
                                break;
                            //break;
                            default:
                                throw new InvalidOperationException("Command type not supported");
                        }
                        sqlCommand.Transaction = null;
                        sqlCommand.Connection = null;
                        // VerboseLog("Database command executed successfully");
                        return result;
                    }, Logger, requestId, useVerboseLogging, cancellationToken); // data connection is received as an argument of the method, calling method should close it


            static bool IsOutputParameter(SqlCommandParameter p)
            {
                return p.ParameterDirection == SqlParameterDirection.Out
                            || p.ParameterDirection == SqlParameterDirection.InOut;
            }
        }


        public async Task<DataSet> ExecuteRoutineAsync(string routineName, List<SqlCommandParameter> parameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(routineName))
            {
                throw new ArgumentException("Stored procedure name can not be blank", nameof(routineName));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return await ExecuteCommandAsync(DBCmdType.DbCmdStoredProc, routineName, null, parameters);
        }

        private async Task<DataSet> GetResultSet(DbCommand command, CancellationToken cancellationToken = default)
        {
            using var dataAdapter = GetNewDataAdapterObject();
            dataAdapter.SelectCommand = command;

            return await Task.Run(() =>
                 {
                     DataSet ds = new DataSet();
                     dataAdapter.Fill(ds);
                     return ds;

                 }, cancellationToken);
        }

        public async Task<IDataReader> GetReaderAsync(string SQL, DbConnection Connection, CommandBehavior commandBehavior)
        {
            using DbCommand Command = GetNewCommandObject();
            Command.Connection = Connection;
            Command.CommandText = SQL;
            return await Command.ExecuteReaderAsync(commandBehavior);
        }

        /// <summary>
        /// Gets a single row from database executing the current SQL command
        /// </summary>
        /// <param name="query">Select SQL command</param>
        /// <param name="activeConnection">Reference of currently open SQL  command</param>
        /// <returns></returns>
        public async Task<IDataReader> GetSingleRowAsync(string query, DbConnection activeConnection, CancellationToken cancellationToken = default)
        => await GetReaderAsync(query, activeConnection, CommandBehavior.SingleRow);

        public string GetDateString(DateTime? date)
        {
            string dateString;
            if (!date.HasValue)
                return DbmsNullText;
            dateString = $"'{date.Value.ToString(DateFormat)}'";
            //if (DBMS == DBMSType.DbOracle)
            //    DateString = "TIMESTAMP " + DateString;

            return dateString;
        }

        /// <summary>
        /// Gets Server Date, Executes SQL to Get the date
        /// </summary>
        public async Task<DateTime> GetServerDateAsync()
        {
            string Sql = null;
            DateTime SerDate = DateTime.MinValue.Date;
            switch (DBMS)
            {
                case DBMSType.DbMsSQLSrv:
                    Sql = "select getdate()";
                    break;
                case DBMSType.DbOracle:
                    Sql = "SELECT SYSDATE FROM DUAL";
                    break;
                case DBMSType.DbMySQL:
                    Sql = "select now()";
                    break;
            }
            if (!string.IsNullOrWhiteSpace(Sql))
            {
                using var connection = await GetNewConnection();
                using (var Reader = await GetReaderAsync(Sql, connection, CommandBehavior.SingleResult))
                {
                    if (Reader.Read())
                    {
                        SerDate = Reader.GetDateTime(0);
                        SerDate = SerDate.Date;
                    }

                    Reader.Close();
                }
                connection.Close();

            }
            return SerDate;
        }

        public static MemoryStream WriteToXML(DataRow Row)
        {
            DataTable Dt;
            MemoryStream TargetStream = new MemoryStream();

            Dt = Row.Table.Clone();
            //newRow = Dt.NewRow();
            Dt.ImportRow(Row);

            //foreach (DataColumn Col in Dt.Columns)
            //    newRow[Col.ColumnName] = Row[Col.ColumnName];
            Dt.TableName = "TempTable";
            //Dt.Rows.Add(newRow);
            Dt.WriteXml(TargetStream, XmlWriteMode.WriteSchema);
            TargetStream.Seek(0, SeekOrigin.Begin);
            return TargetStream;
        }

        public static DataRow ReadRowFromXML(MemoryStream memoryStream)
        {
            DataTable Dt;

            Dt = new DataTable();
            Dt.ReadXml(memoryStream);

            return Dt.Rows.Count == 1 ? Dt.Rows[0] : null;
        }

        protected virtual DbConnection GetNewConnectionObject()
        {
            var connection = DBMS switch
            {
                DBMSType.DbMySQL => new MySqlConnection
                {
                    ConnectionString = GetConnectionString()
                },
                // case DBMSType.DbOracle:
                //     return new OracleConnection();
                // case DBMSType.DbMsSQLSrv:
                //     return new System.Data.SqlClient.SqlConnection();
                _ => throw new Exception("Unable to initialize data connection object instance, provider not known"),
            };
            /*
            The following code works but produces too much logging
                        if (useVerboseLogging)
                        {
                            connection.StateChange += new StateChangeEventHandler(DatabaseConnectionStateChanged);
                            connection.Disposed += objectDisposedHandler;
                        }
                        */
            return connection;
        }

        protected virtual DbCommand GetNewCommandObject()
        {
            var command = DBMS switch
            {
                DBMSType.DbMySQL => new MySqlCommand(),
                // case DBMSType.DbOracle:
                //     return new OracleCommand();
                // case DBMSType.DbMsSQLSrv:
                //     return new System.Data.SqlClient.SqlCommand();
                _ => throw new Exception(),
            };
            /*
          //  The following code works but produces too much logging

                        if (useVerboseLogging)
                        {
                            command.Disposed += objectDisposedHandler;
                        }
            */
            return command;
        }

        internal virtual DbCommand GetNewCommandObject(string query, DbConnection conn)
        {
            //return new OleDbCommand(query, (DbConnection)conn);
            return DBMS switch
            {
                DBMSType.DbMySQL => new MySqlCommand(query, (MySqlConnection)conn),
                // case DBMSType.DbOracle:
                //     return new OracleCommand(query, (OracleConnection)conn);
                // case DBMSType.DbMsSQLSrv:
                //     return new System.Data.SqlClient.SqlCommand(query, (SqlConnection)conn);
                _ => throw new Exception("DBMS type not supported"),
            };
        }

        public virtual DbDataAdapter GetNewDataAdapterObject(string query, DbConnection conn)
        {
            return DBMS switch
            {
                DBMSType.DbMySQL => new MySqlDataAdapter(query, (MySqlConnection)conn),
                // case DBMSType.DbOracle:
                //     return new OracleDataAdapter(query, (OracleConnection)conn);
                // case DBMSType.DbMsSQLSrv:
                //     return new SqlDataAdapter(query, (SqlConnection)conn);
                _ => throw new Exception(),// return new OleDbDataAdapter(query, (OleDbConnection)conn);
            };
        }

        public virtual DbDataAdapter GetNewDataAdapterObject()
        {
            return DBMS switch
            {
                DBMSType.DbMySQL => new MySqlDataAdapter(),
                // case DBMSType.DbOracle:
                //     return new OracleDataAdapter();
                // case DBMSType.DbMsSQLSrv:
                //     return new SqlDataAdapter();
                _ => throw new Exception(),// return new OleDbDataAdapter();
            };
        }

        private void VerboseLog(string message, string messageType = "DataAccess", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (useVerboseLogging)
            {
                Logger.LogDebug(requestId, message, messageType, fields, fileName, memberName, lineNumber);
            }
        }

        private static string DumpCommandValues(DbCommand command)
        {
            return $"Command Type : '{command.CommandType}' Timeout: '{command.CommandTimeout}' Text : '{command.CommandText}'";
        }

        private static string[] DumpSqlParameter(DbParameter parameter)
        {
            return new string[] { parameter.ParameterName, parameter.DbType.ToString(), (parameter.Value ?? "null").ToString() };
        }
    }

}
