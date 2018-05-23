using System;
using Microsoft.Web.XmlTransform;

namespace ConfigTransform
{
  internal class ConsoleLogger : IXmlTransformationLogger
  {
    private readonly bool _verboseLog;

    public ConsoleLogger(bool verboseLog)
    {
      _verboseLog = verboseLog;
    }

    public int ErrorCode { get; private set; }

    public void LogMessage(string message, params object[] messageArgs)
    {
      Console.WriteLine(message, messageArgs);
    }

    public void LogMessage(MessageType type, string message, params object[] messageArgs)
    {
      if (type == MessageType.Verbose && !_verboseLog)
      {
        return;
      }

      Console.WriteLine(message, messageArgs);
    }

    public void LogWarning(string message, params object[] messageArgs)
    {
      ErrorCode = 5;
      Console.WriteLine("\nWarning: " + message, messageArgs);
    }

    public void LogWarning(string file, string message, params object[] messageArgs)
    {
      ErrorCode = 5;
      Console.WriteLine($"\nWarning in {file}.");
      Console.WriteLine(message, messageArgs);
      Console.Error.WriteLine();
    }

    public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
    {
      ErrorCode = 5;
      Console.WriteLine($"\nWarning at (line {lineNumber}, {linePosition}) in {file}.");
      Console.WriteLine(message, messageArgs);
      Console.Error.WriteLine();
    }

    public void LogError(string message, params object[] messageArgs)
    {
      ErrorCode = 6;
      Console.Error.WriteLine("\n" + message, messageArgs);
    }

    public void LogError(string file, string message, params object[] messageArgs)
    {
      ErrorCode = 6;
      Console.Error.WriteLine($"\nError in {file}.");
      Console.Error.WriteLine(message, messageArgs);
      Console.Error.WriteLine();
    }

    public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
    {
      ErrorCode = 6;
      Console.Error.WriteLine($"\nError at (line {lineNumber}, {linePosition}) in {file}.");
      Console.Error.WriteLine(message, messageArgs);
      Console.Error.WriteLine();
    }

    public void LogErrorFromException(Exception ex)
    {
      ErrorCode = 7;
      Console.Error.WriteLine("\n" + ex.Message);
    }

    public void LogErrorFromException(Exception ex, string file)
    {
      ErrorCode = 7;
      Console.Error.WriteLine($"\nError in {file}.");
      Console.Error.WriteLine(ex.Message);
    }

    public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
    {
      ErrorCode = 7;
      Console.Error.WriteLine($"\nError at ({lineNumber},{linePosition}) in {file}.");
      Console.Error.WriteLine(ex.Message);
    }

    public void StartSection(string message, params object[] messageArgs)
    {
      Console.WriteLine("\nStart " + message, messageArgs);
    }

    public void StartSection(MessageType type, string message, params object[] messageArgs)
    {
      if (type == MessageType.Verbose && !_verboseLog)
      {
        return;
      }
      Console.WriteLine("\n" + message, messageArgs);
    }

    public void EndSection(string message, params object[] messageArgs)
    {
      Console.WriteLine(message, messageArgs);
    }

    public void EndSection(MessageType type, string message, params object[] messageArgs)
    {
      if (type == MessageType.Verbose && !_verboseLog)
      {
        return;
      }
      Console.WriteLine(message, messageArgs);
    }
  }
}