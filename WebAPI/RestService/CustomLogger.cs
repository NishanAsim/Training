using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RestService
{
    internal class CustomLogger : Nishan.Logger.ILogger
    {
        public ILogger Logger { get; }

        private string GetFormattedMessage(string requestId, string message, string messageType, string[] fields, string fileName = "", string memberName = "", int lineNumber = 0)
        {
            StringBuilder jsonText = new StringBuilder($"{{\"requestid\" :\"{requestId ?? string.Empty}\",\"messageType\" :\"{messageType}\", \"message\" : \"{message}\"");
            string sourceInformation = $",\"file\" :\"{fileName}\", \"member\" :\"{memberName}\", \"linenumber\" : \"{lineNumber}\"";
            if (fields != null)
            {
                jsonText.Append(", \"fields\" : [");
                string separator = " ";
                for (int i = 0; i < fields.Length && i < 10; i++)
                {
                    jsonText.Append($"{separator}\"{fields[i] ?? string.Empty}\"");
                    separator = ", ";
                }
                jsonText.Append("]");

            }

            jsonText.Append(sourceInformation);
            jsonText.Append("}");

            return jsonText.ToString();
        }

        public CustomLogger(ILoggerFactory loggerFactory)
        {
            if (loggerFactory is null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            //TODO: Change the name here
            Logger = loggerFactory.CreateLogger("ServiceLogger");
        }

        public void LogDebug(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            Logger.LogDebug(new EventId(), null, GetFormattedMessage(requestId, message, messageType ?? "General", fields, fileName, memberName, lineNumber));
        }
        public void LogInfo(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            //TODO: Use appropriate method
            Logger.LogInformation(new EventId(), null, GetFormattedMessage(requestId, message, messageType, fields, fileName, memberName, lineNumber));
        }

        public void LogError(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            //TODO: Use appropriate method
            Logger.LogError(new EventId(), null, GetFormattedMessage(requestId, message, messageType, fields, fileName, memberName, lineNumber));
        }

        public void LogException(string requestId, string message, Exception ex, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            //TODO: Use appropriate method
            Logger.LogCritical(new EventId(), ex, GetFormattedMessage(requestId, message, messageType, fields, fileName, memberName, lineNumber));
        }

        public void LogWarning(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            //TODO: Use appropriate method
            Logger.LogWarning(new EventId(), null, GetFormattedMessage(requestId, message, messageType, fields, fileName, memberName, lineNumber));
        }

    }
}