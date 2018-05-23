using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Web.XmlTransform;

namespace ConfigTransform
{
  /// <summary>
  ///   Transform web.config files.
  /// </summary>
  /// <remarks>Adapted from https://github.com/erichexter/WebConfigTransformRunner/ </remarks>
  static class Program
  {
    const string version = "1.0";

    static void Main(string[] args)
    {
      Console.WriteLine("ConfigTransform v" + version);
      if (args.Length < 3)
      {
        Console.WriteLine("ConfigTransform <ConfigFilename> <TransformFilename> <ResultFilename> /v");
        ExitWith(1);
      }

      var configFilename = args[0];
      var transformFilename = args[1];
      var resultFilename = args[2];
      var verboseLog = args.Length > 3 && args[3].Replace("/", "").ToUpper() == "V";

      if (!File.Exists(configFilename) || !File.Exists(transformFilename))
      {
        Console.WriteLine("The config or transform file do not exist!");
        ExitWith(2);
      }

      Console.WriteLine($"Using '{Path.GetFileName(transformFilename)}' to transform '{Path.GetFileName(configFilename)}'. Output to '{resultFilename}'");

      using (var doc = new XmlTransformableDocument())
      {
        try
        {
          doc.Load(configFilename);
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error loading " + configFilename);
          Console.Error.WriteLine(ex.Message);
          ExitWith(3);
        }

        var logger = new ConsoleLogger(verboseLog);

        using (var tranform = new XmlTransformation(transformFilename, logger))
        {
          try
          {
            if (tranform.Apply(doc))
            {
              doc.Save(resultFilename);
            }
          }
          catch (Exception e)
          {
            Console.Error.WriteLine(e.Message);
            ExitWith(4);
          }

          if (logger.ErrorCode > 0)
          {
            ExitWith(logger.ErrorCode);
          }

          Console.WriteLine("Transformation applied successfully.");
        }
      }
    }

    static void ExitWith(int number)
    {
      Console.Error.WriteLine($"\nExiting with ErrorLevel {number}.\n");
      if (Debugger.IsAttached)
      {
        Debugger.Break();
      }
      Environment.Exit(number);
    }
  }
}