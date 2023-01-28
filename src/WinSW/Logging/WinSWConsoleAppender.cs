using NLog;
using NLog.Targets;
using System;

namespace WinSW.Logging
{
    internal sealed class WinSWConsoleTarget : TargetWithLayoutHeaderAndFooter
    {
        protected override void Write(LogEventInfo logEvent)
        {
            Console.ResetColor();

            var level = logEvent.Level;
            Console.ForegroundColor =
                level >= LogLevel.Error ? ConsoleColor.Red :
                level >= LogLevel.Warn ? ConsoleColor.Yellow :
                level >= LogLevel.Info ? ConsoleColor.Gray :
                ConsoleColor.DarkGray;
            try
            {
                Console.WriteLine(this.RenderLogEvent(this.Layout, logEvent));
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
