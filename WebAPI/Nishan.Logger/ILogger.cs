using System;
using System.Runtime.CompilerServices;

namespace Nishan.Logger
{
    /// <summary>
    /// Represents application logging utility
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a debug event
        /// </summary>
        /// <param name="message">Debug event message data</param>
        // void LogDebug(string requestId, string message, string fileName = "", string memberName ="", int lineNumber = 0 );
        void LogDebug(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);

        void LogInfo(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);

        void LogError(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);

        void LogException(string requestId, string message, Exception ex, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);

        void LogWarning(string requestId, string message, string messageType = "Default", string[] fields = null, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);
    }
}
