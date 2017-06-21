using System;

namespace PlayMe.Plumbing.Diagnostics
{
    public interface ILogger
    {
        void Debug(string format, params object[] args);
        void Error(string format, params object[] args);
        void Fatal(string format, params object[] args);
        void Info(string format, params object[] args);
        void Trace(string format, params object[] args);
        void Warn(string format, params object[] args);

        void DebugException(string message, Exception exception);
        void InfoException(string message, Exception exception);
        void ErrorException(string message, Exception exception);
        void FatalException(string message, Exception exception);


        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsTraceEnabled { get; }
        bool IsWarnEnabled { get; }
    }
}
