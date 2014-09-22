using System;
using System.Collections.Generic;
using int32.Utils.Extensions;
using int32.Utils.Logger.Contracts;
using int32.Utils.Logger.Messages;

namespace int32.Utils.Logger
{
    public class Logger : IDisposable
    {
        private readonly List<ILogger> _loggers;

        public LogConfig Config { get; internal set; }

        public Logger(params ILogger[] loggers)
        {
            _loggers = new List<ILogger>();
            _loggers.AddRange(loggers);

            Config = new LogConfig
            {
                EnableDebugging = true,
                EnableErrors = true,
                EnableInfos = true,
                EnableWarnings = true
            };
        }

        public void Dispose()
        {
            _loggers.ForEach(i => i.Dispose());
        }

        public void Info(string message)
        {
            Log(new InfoMessage(message) { Format = Config.Format });
        }

        public void Warn(string message)
        {
            Log(new WarnMessage(message) { Format = Config.Format });
        }

        public void Error(Exception ex)
        {
            Log(new ErrorMessage(ex) { EnableDebugging = Config.EnableDebugging, Format = Config.Format });
        }

        private void Log(BaseMessage msg)
        {
            if (msg.Is<InfoMessage>() && Config.EnableInfos)
                _loggers.ForEach(i => i.Info((InfoMessage)msg));

            if (msg.Is<WarnMessage>() && Config.EnableWarnings)
                _loggers.ForEach(i => i.Warn((WarnMessage)msg));

            if (msg.Is<ErrorMessage>() && Config.EnableErrors)
                _loggers.ForEach(i => i.Error((ErrorMessage)msg));
        }
    }
}
