using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nishan.Logger;

namespace Nishan.Data
{
    internal static class CommandExecutor
    {
        internal static async Task<T> ExecuteDbTask<T>(Func<CancellationToken, Task<T>> dbMethod, ILogger logger,string requestId, bool useVerboseLogging, CancellationToken cancellationToken = default)
        {
            try
            {
               var result = await dbMethod(cancellationToken);
                if(useVerboseLogging)
                {
                    logger.LogDebug(requestId, "Command executed successfully", "DataAccess");
                }
               return result;
            }
            catch (MySqlException ex) when (ex.SqlState == "45001") // Data Validation Exception
            {
                logger.LogError(requestId,"Database command failed due to data validation error, see exception below for details");
                throw SqlQueryArgumentException.GetSqlQueryArgumentException(ex.SqlState, ex.Message, ex);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                logger.LogError(requestId,"Database command failed due to invalid database parameters, see exception below for details");
                throw SqlQueryArgumentException.GetSqlQueryArgumentException(ex.SqlState,
                                                                             ex.Message,
                                                                             string.Empty,
                                                                             SqlCommandErrorType.DuplicateKey,
                                                                             ex);
            }
            catch (MySqlException ex) when (ex.Number == 1452)
            {
                logger.LogError(requestId,"Database command failed due to an unknown reason, see exception below for details");
                throw SqlQueryArgumentException.GetSqlQueryArgumentException(ex.SqlState,
                                                                             ex.Message,
                                                                             string.Empty,
                                                                             SqlCommandErrorType.InvalidReferenceValue,
                                                                             ex);
            }
        }
    }
}