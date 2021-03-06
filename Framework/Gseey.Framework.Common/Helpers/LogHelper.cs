﻿namespace Gseey.Framework.Common.Helpers
{
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using NLog;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 日志帮助类 默认开启log4net的日志debug模式,如需关闭,配置文件添加节点即可: <add key="log4net.Internal.Quiet" value="true"/>
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// log4net对象
        /// </summary>
        //private static ILog _log = null;

        /// <summary>
        /// appender集合
        /// </summary>
        private static ConcurrentDictionary<string, IAppender> _AppenderDictionary = new ConcurrentDictionary<string, IAppender>();

        /// <summary>
        /// 默认的日志位置，在配置文件中配置
        /// </summary>
        private static string _controlLogFileName = string.Empty;

        /// <summary>
        /// 线程信号
        /// </summary>
        private static AutoResetEvent _signal = new AutoResetEvent(false);

        /// <summary>
        /// 日志消息队列
        /// </summary>
        private static ConcurrentQueue<LogMessage> _msgQueue = new ConcurrentQueue<LogMessage>();

        /// <summary>
        /// 默认的lognet日志地址
        /// </summary>
        private static string _logPath = ConfigHelper.Get("LogPath");

        /// <summary>
        /// Gets a value indicating whether DisableInfoLog
        /// 是否禁用记录info级别日志
        /// </summary>
        private static bool DisableInfoLog
        {
            get
            {
                return ConfigHelper.Get("DisableInfoLog", "0").ToInt() == 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether DisableDebugLog
        /// 是否禁用记录debug级别日志
        /// </summary>
        private static bool DisableDebugLog
        {
            get
            {
                return ConfigHelper.Get("DisableDebugLog", "0").ToInt() == 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether DisableErrorLog
        /// 是否禁用记录error级别日志
        /// </summary>
        private static bool DisableErrorLog
        {
            get
            {
                return ConfigHelper.Get("DisableErrorLog", "0").ToInt() == 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether DisableWarnLog
        /// </summary>
        private static bool DisableWarnLog
        {
            get
            {
                return ConfigHelper.Get("DisableWarnLog", "0").ToInt() == 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether DisableFatalLog
        /// </summary>
        private static bool DisableFatalLog
        {
            get
            {
                return ConfigHelper.Get("DisableFatalLog", "0").ToInt() == 1;
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="LogHelper"/> class.
        /// </summary>
        static LogHelper()
        {
            if (string.IsNullOrEmpty(_logPath))
            {
                _logPath = "";
            }

            Task.Run(() =>
            {
                while (true)
                {
                    //等待线程信号
                    _signal.WaitOne();

                    LogMessage msg;
                    while (_msgQueue.Count > 0 && _msgQueue.TryDequeue(out msg))
                    {
                        WriteLog(msg.LogLevel, msg.Message, msg.Exception, msg.BizEnum, msg.LogTag, msg.IsShowConsole);
                    }

                    //重置线程信号
                    _signal.Reset();
                }
            });
        }

        /// <summary>
        /// 记录Debug级别日志
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Debug(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog)
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Debug,
                    LogTag = "",
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog(LogLevelEnum.Debug, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Debug级别日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Debug<T>(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog) where T : class
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Debug,
                    LogTag = typeof(T).FullName,
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog<T>(LogLevelEnum.Debug, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Error级别日志
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Error(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog)
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Error,
                    LogTag = "",
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog(LogLevelEnum.Error, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Error级别日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Error<T>(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog) where T : class
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Error,
                    LogTag = typeof(T).FullName,
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog<T>(LogLevelEnum.Error, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Info级别日志
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Info(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog)
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Info,
                    LogTag = "",
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog(LogLevelEnum.Info, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Info级别日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Info<T>(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog) where T : class
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Info,
                    LogTag = typeof(T).FullName,
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog<T>(LogLevelEnum.Info, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Fatal级别日志
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Fatal(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog)
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Fatal,
                    LogTag = "",
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog(LogLevelEnum.Fatal, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Fatal级别日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Fatal<T>(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog) where T : class
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Fatal,
                    LogTag = typeof(T).FullName,
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog<T>(LogLevelEnum.Fatal, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Warn级别日志
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Warn(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog)
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Warn,
                    LogTag = "",
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog(LogLevelEnum.Warn, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录Warn级别日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void Warn<T>(string message, Exception exception = null, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool Enqueue = false, LogType logType = LogType.Nlog) where T : class
        {
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = false,
                    LogLevel = LogLevelEnum.Warn,
                    LogTag = typeof(T).FullName,
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog<T>(LogLevelEnum.Warn, message, exception, bizEnum: bizEnum);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception">The exception<see cref="Exception"/></param>
        /// <param name="logLevel"></param>
        /// <param name="bizEnum"></param>
        /// <param name="isShowConsole"></param>
        /// <param name="folderName"></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void RunLog(string message, Exception exception = null, LogLevelEnum logLevel = LogLevelEnum.Info, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool isShowConsole = false, string folderName = "", bool Enqueue = false, LogType logType = LogType.Nlog)
        {
            Console.WriteLine("==================RunLog=====================");
            if (Enqueue)
            {
                var msg = new LogMessage
                {
                    Message = message,
                    Exception = exception,
                    BizEnum = bizEnum,
                    IsShowConsole = isShowConsole,
                    LogLevel = logLevel,
                    LogTag = folderName,
                    LogType = logType
                };
                _msgQueue.Enqueue(msg);
                _signal.Set();
            }
            else
            {
                WriteLog(logLevel, message, exception, bizEnum, folderName, isShowConsole, logType);
            }
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="bizEnum"></param>
        /// <param name="isShowConsole"></param>
        /// <param name="Enqueue">The Enqueue<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        public static void WriteExceptionLog(this Exception ex, string message = "", LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, bool isShowConsole = true, bool Enqueue = false, LogType logType = LogType.Nlog)
        {
            RunLog(message, ex, logLevel: LogLevelEnum.Error, bizEnum: bizEnum, isShowConsole: isShowConsole, folderName: "", Enqueue: Enqueue, logType: logType);
        }

        /// <summary>
        /// 获取lognet的日志level
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private static Level GetLogNetLevel(LogLevelEnum logLevel)
        {
            Level level = Level.Info;
            switch (logLevel)
            {
                case LogLevelEnum.Info:
                    level = Level.Info;
                    break;
                case LogLevelEnum.Error:
                    level = Level.Error;
                    break;
                case LogLevelEnum.Warn:
                    level = Level.Warn;
                    break;
                case LogLevelEnum.Fatal:
                    level = Level.Fatal;
                    break;
                case LogLevelEnum.Debug:
                    level = Level.Debug;
                    break;
            }
            return level;
        }

        /// <summary>
        /// 根据业务类型分配日志目录
        /// </summary>
        /// <param name="bizEnum"></param>
        /// <returns></returns>
        private static string GetBussinussLogFilePath(LogicBuissnussEnum bizEnum)
        {
            var folderPath = string.Format("{0}Log", bizEnum.ToString());
            return folderPath;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Tag">The Tag<see cref="string"/></param>
        /// <param name="isShowConsole">The isShowConsole<see cref="bool"/></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        private static void WriteLog(LogLevelEnum logLevel, string message, Exception exception, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, string Tag = "", bool isShowConsole = false, LogType logType = LogType.Nlog)
        {
            try
            {
                Console.WriteLine("==================WriteLog=====================");
                //var logMsg = GetExceptionMsg(exception, message.ToString());

                //var folderName = string.IsNullOrEmpty(Tag) ? GetBussinussLogFilePath(bizEnum) : Tag;

                //WriteCustomLog(MethodBase.GetCurrentMethod().DeclaringType, logLevel, logMsg, logType, folderName);

                //SetConsoleForegroundColor(logLevel, message, isShowConsole);
            }
            catch (Exception ex)
            {
                var newEx = new Exception("LogHelper记录日志出错", ex);
            }
        }

        /// <summary>
        /// 设置控制台的文字展示颜色
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="isShowConsole"></param>
        private static void SetConsoleForegroundColor(LogLevelEnum logLevel, string message, bool isShowConsole)
        {
            if (isShowConsole)
            {
                if (logLevel == LogLevelEnum.Debug)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (logLevel == LogLevelEnum.Info)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (logLevel == LogLevelEnum.Warn)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (logLevel == LogLevelEnum.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (logLevel == LogLevelEnum.Fatal)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="logType">The logType<see cref="LogType"/></param>
        /// <param name="folderName">The folderName<see cref="string"/></param>
        private static void WriteCustomLog(Type type, LogLevelEnum logLevel, string message, LogType logType = LogType.Nlog, string folderName = "")
        {
            switch (logType)
            {
                case LogType.Log4Net:
                default:
                    {
                        var level = GetLogNetLevel(logLevel);
                        RecoredLog4Net(type, level, message, folderName);
                    }
                    break;
                case LogType.Nlog:
                    {
                        var level = GetNlogLevel(logLevel);
                        RecoredNlog(level, message);
                    }
                    break;
            }
        }

        /// <summary>
        /// 使用nlog记录日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        private static void RecoredNlog(LogLevel level, string message)
        {
            var formatMsg = string.Format("\n========[start]========\n{0}\n========[end]========", message);

            var log = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

            if (level == LogLevel.Info && !DisableInfoLog)
            {
                log.Info(formatMsg);
            }
            else if (level == LogLevel.Error && !DisableErrorLog)
            {
                log.Error(formatMsg);
            }
            else if (level == LogLevel.Debug && !DisableDebugLog)
            {
                log.Debug(formatMsg);
            }
            else if (level == LogLevel.Warn && !DisableWarnLog)
            {
                log.Warn(formatMsg);
            }
            else if (level == LogLevel.Fatal && !DisableFatalLog)
            {
                log.Fatal(formatMsg);
            }
        }

        /// <summary>
        /// 获取nlog的日志级别
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private static NLog.LogLevel GetNlogLevel(LogLevelEnum logLevel)
        {
            switch (logLevel)
            {
                case LogLevelEnum.Info:
                default:
                    return NLog.LogLevel.Info;
                case LogLevelEnum.Error:
                    return NLog.LogLevel.Error;
                case LogLevelEnum.Warn:
                    return NLog.LogLevel.Warn;
                case LogLevelEnum.Fatal:
                    return NLog.LogLevel.Fatal;
                case LogLevelEnum.Debug:
                    return NLog.LogLevel.Debug;
            }
        }

        /// <summary>
        /// 记录log4net日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="folderName">The folderName<see cref="string"/></param>
        private static void RecoredLog4Net(Type type, Level logLevel, string message, string folderName = "")
        {
            LoadAppender(folderName);
            var respName = "NETCoreRepository";
            var name = "";
            log4net.Repository.ILoggerRepository repository = null;
            try
            {
                repository = log4net.LogManager.GetRepository(respName);
                if (repository == null)
                    repository = log4net.LogManager.CreateRepository(respName);
                else
                {
                    repository.Exists(name);
                }
            }
            catch (Exception)
            {
                if (repository == null)
                    repository = log4net.LogManager.CreateRepository(respName);
            }

            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            var log = log4net.LogManager.GetLogger(respName, type.Name);

            if (logLevel == Level.Info && !DisableInfoLog)
            {
                log.Info(message);
            }
            else if (logLevel == Level.Error && !DisableErrorLog)
            {
                log.Error(message);
            }
            else if (logLevel == Level.Debug && !DisableDebugLog)
            {
                log.Debug(message);
            }
            else if (logLevel == Level.Warn && !DisableWarnLog)
            {
                log.Warn(message);
            }
            else if (logLevel == Level.Fatal && !DisableFatalLog)
            {
                log.Fatal(message);
            }
        }

        /// <summary>
        /// 获取异常消息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message">The message<see cref="string"/></param>
        /// <returns></returns>
        private static string GetExceptionMsg(Exception exception = null, string message = "")
        {
            StringBuilder builder;
            var msg = string.Empty;
            var paramsMsg = string.Empty;

            if (exception != null)//有异常才会记录相关信息
            {
                msg += GetStactTraceMsg();
                //msg += GetRequestMsg( == null ? null : HttpContext.Current.Request);
            }

            var count = 0;
            while (exception != null || (exception != null && exception.InnerException != null))
            {
                count++;
                builder = new StringBuilder();

                builder.Append("\n\t异常层数：" + count);
                builder.Append("\n\t错误信息：" + exception.Message);
                builder.Append("\n\t异常类型：" + exception.GetType().FullName);

                if (exception is ArgumentNullException)
                {
                    builder.Append("\n\t异常参数名：" + ((ArgumentNullException)exception).ParamName);
                }
                else if (exception is ArgumentOutOfRangeException)
                {
                    builder.Append("\n\t异常参数名：" + ((ArgumentOutOfRangeException)exception).ParamName);
                    builder.Append("\n\t异常参数值：" + ((ArgumentOutOfRangeException)exception).ActualValue);
                }
                else if (exception is WebException)
                {
                    var exp = ((WebException)exception);
                    builder.Append("\n\tWeb返回值：" + exp.Status.ToString());
                    builder.Append("\n\tWeb异常路径：" + exp.Response.ResponseUri.AbsoluteUri);
                }

                builder.Append("\n\t错误源：" + exception.Source);
                builder.Append("\n\t异常方法：" + exception.TargetSite);
                builder.Append("\n\t堆栈信息：" + exception.StackTrace);
                msg += builder.ToString();
                exception = exception.InnerException;
            }
            return string.Format("{0}{1}", message, msg);
        }

        /// <summary>
        /// 记录request请求信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetRequestMsg(HttpRequest request = null)
        {
            var msg = string.Empty;
            //记录request的参数
            if (request != null)
            {

                msg += string.Format("\n\t参数信息{3}\n\t页面地址:{0}\n\t调用方式:{1}\n\tUrlReferrer:{2}", request.Path, request.Method.ToUpper(), request.Headers.ToJson(), GetNameValueCollection(request));
            }

            return msg;
        }

        /// <summary>
        /// 获取传输过来的参数及参数值
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/></param>
        /// <returns></returns>
        private static string GetNameValueCollection(HttpRequest request)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var paramDict = new Dictionary<string, StringValues>();
                foreach (var item in request.Form)
                {
                    paramDict.Add(item.Key, item.Value);
                }
                foreach (var item in request.Query)
                {
                    paramDict.Add(item.Key, item.Value);
                }

                builder.AppendFormat("\r\n\t\t参数名称\t\t参数值");
                foreach (string key in paramDict.Keys)
                {
                    var value = paramDict[key];
                    builder.AppendFormat("\r\n\t\t{0}\t\t{1}", key, value);
                }
                return builder.ToString();
            }
            catch (Exception)
            {
                return "\r\n\t\t参数名称\t\t参数值";
            }
        }

        /// <summary>
        /// 记录堆栈信息
        /// </summary>
        /// <returns></returns>
        private static string GetStactTraceMsg()
        {
            var msg = string.Empty;

            var canRecordStackMsg = ConfigHelper.Get("CanRecordStackMsg", "0") == "1";
            if (canRecordStackMsg)//开启记录堆栈信息
            {
                var stackTraceMsg = new List<string>();
                StackTrace stackTrace = new StackTrace(true);
                var stackFrames = stackTrace.GetFrames();
                foreach (var item in stackFrames)
                {
                    var mBase = item.GetMethod();
                    if (mBase == null
                        || mBase.DeclaringType == null
                        || mBase.DeclaringType.Assembly == null)
                    {
                        continue;
                    }
                    var pramStr = new List<string>();
                    var prams = mBase.GetParameters();
                    foreach (var p in prams)
                    {
                        pramStr.Add(string.Format("\t\tName={0},Type={1}", p.Name, p.ParameterType));
                    }
                    var fileLine = item.GetFileLineNumber();
                    var columnLine = item.GetFileColumnNumber();
                    stackTraceMsg.Add(string.Format("\t在 {0}.{1} {2} {3} {4}", mBase.DeclaringType.FullName, mBase.Name, fileLine == 0 ? string.Empty : "第" + fileLine + "行", columnLine == 0 ? string.Empty : "第" + columnLine + "列", pramStr.Count <= 0 ? string.Empty : "参数信息:\r" + string.Join("\n", pramStr.ToArray())));
                }
                if (stackTraceMsg.Count > 0)
                {
                    msg += string.Format("\t调用堆栈信息:\r{0}\r\t-------------------------------\r", string.Join("\n", stackTraceMsg.ToArray()));
                }
            }

            return msg;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">消息内容</param>
        /// <param name="exception">异常信息</param>
        /// <param name="bizEnum">The bizEnum<see cref="LogicBuissnussEnum"/></param>
        /// <param name="Tag">The Tag<see cref="string"/></param>
        /// <param name="isShowConsole">The isShowConsole<see cref="bool"/></param>
        private static void WriteLog<T>(LogLevelEnum logLevel, string message, Exception exception, LogicBuissnussEnum bizEnum = LogicBuissnussEnum.Default, string Tag = "", bool isShowConsole = false)
        {
            WriteLog(logLevel, message, exception, bizEnum, typeof(T).FullName, isShowConsole);
        }

        /// <summary>
        /// 根据业务类型,获取不同的业务appender
        /// </summary>
        /// <param name="folderName">The folderName<see cref="string"/></param>
        private static void LoadAppender(string folderName)
        {
            var appender = _AppenderDictionary.GetOrAdd(folderName, m =>
            {

                //Hierarchy hier = LogManager.GetRepository(Guid.NewGuid().ToString()) as Hierarchy;

                RollingFileAppender fileAppender = new RollingFileAppender();
                fileAppender.Name = "RollingFileAppender";
                fileAppender.File = string.Format("{0}\\{3}\\{1}\\{2}.log", _logPath, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("dd_HH"), folderName);
                fileAppender.AppendToFile = true;

                PatternLayout patternLayout = new PatternLayout();
                patternLayout.Header = "\n========【START】==========";
                patternLayout.Footer = "\n========【END】==========";
                patternLayout.ConversionPattern = "%n【记录时间】：%date 【日志级别】：%-5level - 【日志内容】：%message";
                patternLayout.ActivateOptions();
                fileAppender.Layout = patternLayout;
                fileAppender.StaticLogFileName = true;
                fileAppender.LockingModel = new FileAppender.MinimalLock() { CurrentAppender = fileAppender };
                fileAppender.MaxSizeRollBackups = 5;
                fileAppender.MaximumFileSize = "1GB";
                fileAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;

                //选择UTF8编码，确保中文不乱码。
                fileAppender.Encoding = Encoding.UTF8;

                fileAppender.ActivateOptions();
                return fileAppender;
            });
        }

        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// Defines the Log4Net
            /// </summary>
            Log4Net = 10,
            /// <summary>
            /// Defines the Nlog
            /// </summary>
            Nlog = 20,
        }

        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogLevelEnum
        {
            /// <summary>
            /// Defines the Info
            /// </summary>
            Info,
            /// <summary>
            /// Defines the Error
            /// </summary>
            Error,
            /// <summary>
            /// Defines the Warn
            /// </summary>
            Warn,
            /// <summary>
            /// Defines the Fatal
            /// </summary>
            Fatal,
            /// <summary>
            /// Defines the Debug
            /// </summary>
            Debug,
        }

        /// <summary>
        /// 业务类型（随着开发业务类型增加而增加枚举）
        /// </summary>
        public enum LogicBuissnussEnum
        {
            /// <summary>
            /// 默认（不区分）业务
            /// </summary>
            Default = 0,
            /// <summary>
            /// 通用业务
            /// </summary>
            Common = 10,
            /// <summary>
            /// 用户登录业务
            /// </summary>
            UserLogin = 20,
            /// <summary>
            /// 用户权限业务
            /// </summary>
            UserPermission = 30,
            /// <summary>
            /// 微信业务
            /// </summary>
            Wechat = 40,
            /// <summary>
            /// 记账业务
            /// </summary>
            Account = 50,
            /// <summary>
            /// 红酒业务
            /// </summary>
            Wine = 60,
            /// <summary>
            /// 框架日志
            /// </summary>
            Framework = 70,
            /// <summary>
            /// 系统日志
            /// </summary>
            System = 80,
            /// <summary>
            /// 看书世界
            /// </summary>
            Kanshushijie = 90,
            /// <summary>
            /// 小说
            /// </summary>
            Novel = 100
        }

        /// <summary>
        /// 日志消息
        /// </summary>
        private class LogMessage
        {
            /// <summary>
            /// Gets or sets the Message
            /// 消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Gets or sets the Exception
            /// 异常信息
            /// </summary>
            public Exception Exception { get; set; }

            /// <summary>
            /// Gets or sets the LogLevel
            /// 日志级别
            /// </summary>
            public LogLevelEnum LogLevel { get; set; }

            /// <summary>
            /// Gets or sets the BizEnum
            /// 业务类型
            /// </summary>
            public LogicBuissnussEnum BizEnum { get; set; }

            /// <summary>
            /// Gets or sets the LogTag
            /// 日志标签
            /// </summary>
            public string LogTag { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether IsShowConsole
            /// 是否在控制台展示
            /// </summary>
            public bool IsShowConsole { get; set; }

            /// <summary>
            /// Gets or sets the LogType
            /// 日志类型
            /// </summary>
            public LogType LogType { get; set; }
        }
    }
}
