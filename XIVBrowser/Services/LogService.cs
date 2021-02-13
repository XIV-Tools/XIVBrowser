// © XIV-Tools.
// Licensed under the MIT license.

namespace XIVBrowser
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.ExceptionServices;
	using System.Runtime.InteropServices;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;
	using Serilog;
	using Serilog.Core;
	using Serilog.Events;

	public static class LogService
	{
		public static string LogDirectory;

		private static readonly LoggingLevelSwitch LogLevel = new LoggingLevelSwitch()
		{
			MinimumLevel = LogEventLevel.Verbose,
		};

		public static void Create()
		{
			AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
			Application.Current.Dispatcher.UnhandledException += (s, e) => Log.Fatal(e.Exception, e.Exception.Message);
			Application.Current.DispatcherUnhandledException += (s, e) => Log.Fatal(e.Exception, e.Exception.Message);
			TaskScheduler.UnobservedTaskException += (s, e) => Log.Fatal(e.Exception, e.Exception.Message);

			LogDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

			LoggerConfiguration config = new LoggerConfiguration();
			config.MinimumLevel.ControlledBy(LogLevel);
			config.WriteTo.File(LogDirectory + "/log.txt");
			config.WriteTo.Sink<ErrorDialogLogDestination>();
			config.WriteTo.Debug();

			Serilog.Log.Logger = config.CreateLogger();

			Log.Information("OS: " + RuntimeInformation.OSDescription, "Info");
			Log.Information("Framework: " + RuntimeInformation.FrameworkDescription, "Info");
			Log.Information("OS Architecture: " + RuntimeInformation.OSArchitecture.ToString(), "Info");
			Log.Information("Process Architecture: " + RuntimeInformation.ProcessArchitecture.ToString(), "Info");
		}

		private static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;

			if (ex == null)
				return;

			Log.Fatal(ex, ex.Message);
		}

		private class ErrorDialogLogDestination : ILogEventSink
		{
			public void Emit(LogEvent logEvent)
			{
				if (logEvent.Level >= LogEventLevel.Error)
				{
					////ErrorDialog.ShowError(ExceptionDispatchInfo.Capture(new Exception(logEvent.MessageTemplate.Text, logEvent.Exception)), logEvent.Level == LogEventLevel.Fatal);
				}
			}
		}
	}
}
