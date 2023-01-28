using System.Diagnostics;
using NLog;
using NLog.Targets;

namespace WinSW.Logging
{
    /// <summary>
    /// Implementes service Event log appender for log4j.
    /// The implementation presumes that service gets initialized after the logging.
    /// </summary>
    internal sealed class ServiceEventLogTarget : TargetWithLayoutHeaderAndFooter
    {
        private readonly WrapperServiceEventLogProvider provider;

        internal ServiceEventLogTarget(WrapperServiceEventLogProvider provider)
        {
            this.provider = provider;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var eventLog = this.provider.Locate();

            if (eventLog is not null)
            {
                eventLog.WriteEntry(this.RenderLogEvent(this.Layout, logEvent), ToEventLogEntryType(logEvent.Level));
                return;
            }

            try
            {
                using var backupLog = new EventLog("Application", ".", "Windows Service Wrapper");
                backupLog.WriteEntry(this.RenderLogEvent(this.Layout, logEvent), ToEventLogEntryType(logEvent.Level));
            }
            catch
            {
            }
        }

        private static EventLogEntryType ToEventLogEntryType(LogLevel level)
        {
            if (level >= LogLevel.Error)
            {
                return EventLogEntryType.Error;
            }

            if (level >= LogLevel.Warn)
            {
                return EventLogEntryType.Warning;
            }

            // All other events will be posted as information
            return EventLogEntryType.Information;
        }
    }
}
