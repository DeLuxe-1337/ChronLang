using ChronIR;
using System.Diagnostics;

namespace ChronCompiler.Targets
{
    internal static class TargetCompiler
    {
        private static string RootDirectory = AppContext.BaseDirectory;
        private static string WorkingDirectory = Environment.CurrentDirectory;
        private static Dictionary<string, string> argumentsForTarget = new();
        public static void AddTargetArgument(string name, string value) => argumentsForTarget[name] = value;
        public static void CompileTarget(ChronModule module, Target target)
        {
            ChronContext context = module.GetContext();

            string sourceFilePath = Path.Combine(WorkingDirectory, $"{context.Name}.chron.c");
            string targetFilePath = Path.Combine(RootDirectory, $"{context.Name}.chron.c");

            string compiledExecutable = Path.Combine(RootDirectory, $"{context.Name}.chron.exe");
            string outputPath = Path.Combine(WorkingDirectory, $"{context.Name}.chron.exe");

            {
                if (!File.Exists(targetFilePath))
                    File.Copy(sourceFilePath, targetFilePath);

                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                if (File.Exists(compiledExecutable))
                    File.Delete(compiledExecutable);
            }

            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WorkingDirectory = WorkingDirectory;

                process.StartInfo.FileName = target.Path;

                process.StartInfo.EnvironmentVariables["CHRON_NAME"] = context.Name;
                process.StartInfo.EnvironmentVariables["CHRON_WORKING_DIR"] = WorkingDirectory;
                process.StartInfo.EnvironmentVariables["CHRON_BACKEND"] = Path.Combine(RootDirectory, "Backend");
                process.StartInfo.EnvironmentVariables["CHRON_SOURCE_FILE"] = Path.Combine(RootDirectory, $"{context.Name}.chron.c");
                process.StartInfo.EnvironmentVariables["CHRON_STATIC_LIBS"] = string.Join(" ", module.GetStaticLink().Values);

                foreach (var argument in argumentsForTarget)
                {
                    process.StartInfo.EnvironmentVariables[argument.Key] = argument.Value;
                    Console.WriteLine(argument.Value);
                    Console.WriteLine(argument.Key);
                }

                process.Start();
                process.WaitForExit();
            }

            {
                if (File.Exists(compiledExecutable) && !File.Exists(outputPath))
                    File.Copy(compiledExecutable, outputPath);

                if (File.Exists(compiledExecutable) && compiledExecutable != outputPath)
                    File.Delete(compiledExecutable);

                if (File.Exists(targetFilePath) && targetFilePath != sourceFilePath)
                    File.Delete(targetFilePath);
            }
        }
    }
}
