using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Nishan.Data
{
    internal static class DbCommandExtensions
    {
        internal static async Task CheckAndExecuteNonQueryAsync(this IDbCommand objCmd, CancellationToken cancellationToken = default)
        {
            if (objCmd is DbCommand command)
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            else
            {
                objCmd.ExecuteNonQuery();
            }
        }
        
    }
}