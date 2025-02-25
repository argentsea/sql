using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArgentSea.Sql.Test
{
    internal class Disposee : IDisposable
    {
        public void Dispose()
        {
            Debug.WriteLine("Disposee.Dispose");
        }
    }

    internal class DebugLogger : Microsoft.Extensions.Logging.ILogger
    {
        private StringBuilder stringBuilder = new();
        public bool IsTraceEnabled => true;
        public bool IsDebugEnabled => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;
        public bool IsInfoEnabled => true;
        public bool IsWarnEnabled => true;

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return new Disposee();
        }

        public ILogger CreateChildLogger(string loggerName) => throw new NotImplementedException();
        public void Debug(string message) => this.stringBuilder.AppendLine(message);
        public void Debug(Func<string> messageFactory) => this.stringBuilder.AppendLine(messageFactory());
        public void Debug(string message, Exception exception) => this.stringBuilder.AppendLine(message);
        public void DebugFormat(string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void DebugFormat(Exception exception, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void Error(string message) => this.stringBuilder.AppendLine(message);
        public void Error(Func<string> messageFactory) => this.stringBuilder.AppendLine(messageFactory());
        public void Error(string message, Exception exception) => this.stringBuilder.AppendLine(message);
        public void ErrorFormat(string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void ErrorFormat(Exception exception, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void Fatal(string message) => this.stringBuilder.AppendLine(message);
        public void Fatal(Func<string> messageFactory) => this.stringBuilder.AppendLine(messageFactory());
        public void Fatal(string message, Exception exception) => this.stringBuilder.AppendLine(message);
        public void FatalFormat(string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void FatalFormat(Exception exception, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void Info(string message) => this.stringBuilder.AppendLine(message);
        public void Info(Func<string> messageFactory) => this.stringBuilder.AppendLine(messageFactory());
        public void Info(string message, Exception exception) => this.stringBuilder.AppendLine(message);
        public void InfoFormat(string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void InfoFormat(Exception exception, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => this.stringBuilder.AppendLine(exception?.Message);

        public void Trace(string message) => this.stringBuilder.AppendLine(message);
        public void Trace(Func<string> messageFactory) => this.stringBuilder.AppendLine(messageFactory());
        public void Trace(string message, Exception exception) => this.stringBuilder.AppendLine(message);
        public void TraceFormat(string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void TraceFormat(Exception exception, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void TraceFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void Warn(string message) => this.stringBuilder.AppendLine(message);
        public void Warn(Func<string> messageFactory) => this.stringBuilder.AppendLine(messageFactory());
        public void Warn(string message, Exception exception) => this.stringBuilder.AppendLine(message);
        public void WarnFormat(string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void WarnFormat(Exception exception, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(format, args));
        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));
        public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args) => this.stringBuilder.AppendLine(string.Format(formatProvider, format, args));

        public string LogResult { get => stringBuilder.ToString(); }
    }
}
