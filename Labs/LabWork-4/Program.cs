using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using LibraryForLab4;
using System.Runtime.InteropServices;
using System.IO;
using dotenv.net;


namespace LabWork_4
{
    class Program
    {
        private static readonly string envVar = "LAB_PATH";
        private static readonly string outputFileName = "output.txt";
        private static readonly string inputFileName = "input.txt";

        public static string FindFile(string fileType)
        {
            string path = "";
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config", "my-tool.cfg")))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    String[] temp = File.ReadAllText(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config", "my-tool.cfg")).Split(" ");
                    Environment.SetEnvironmentVariable(temp[0], temp[1]);
                }
                else
                {
                    DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config", "my-tool.cfg") }));
                }
                path = Path.Combine(Environment.GetEnvironmentVariable(envVar), fileType);
            }
            else if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileType)))
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileType);
            }
            return path;
        }

        public static void ExecuteLab(ILab lab, CommandOption input, CommandOption output)
        {
            if (input.HasValue())
                lab.Input = input.Value();
            else 
                lab.Input = FindFile(inputFileName);

            if (output.HasValue())
                lab.Output = output.Value();
            else
                lab.Output = FindFile(outputFileName);

            if (lab.Input != "" && lab.Output != "")
            {
                lab.Run();
            }
            else
            {
                if (lab.Input == "")
                    Console.WriteLine("Input file not found.");
                if (lab.Output == "")
                    Console.WriteLine("Output file not found.");
            }
        }

        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "lab4",
                Description = "Console application lab4",
            };

            app.HelpOption(inherited: true);

            app.Command("version", configCmd =>
            {
                configCmd.OnExecute(() =>
                {
                    Console.WriteLine("Author: Hoda Mykhailo");
                    Console.WriteLine("Version: 0.0.10");
                    return 1;
                });
            });

            app.Command("run", configCmd =>
            {
                configCmd.OnExecute(() =>
                {
                    Console.WriteLine("Specify a lab to execute.");
                    return 1;
                });

                configCmd.Command("lab1", setCmd =>
                {
                    setCmd.Description = "Execute lab1";
                    var input = setCmd.Option("-i|--input", "input", CommandOptionType.SingleValue);
                    var output = setCmd.Option("-o|--output", "output", CommandOptionType.SingleValue);

                    setCmd.OnExecute(() =>
                    {
                        ILab lab = new Lab1();
                        ExecuteLab(lab, input, output);
                    });
                });

                configCmd.Command("lab2", setCmd =>
                {
                    setCmd.Description = "Execute lab2";
                    var input = setCmd.Option("-i|--input", "input", CommandOptionType.SingleValue);
                    var output = setCmd.Option("-o|--output", "output", CommandOptionType.SingleValue);

                    setCmd.OnExecute(() =>
                    {
                        ILab lab = new Lab2();
                        ExecuteLab(lab, input, output);
                    });
                });

                configCmd.Command("lab3", setCmd =>
                {
                    setCmd.Description = "Execute lab3";
                    var input = setCmd.Option("-i|--input", "input", CommandOptionType.SingleValue);
                    var output = setCmd.Option("-o|--output", "output", CommandOptionType.SingleValue);

                    setCmd.OnExecute(() =>
                    {
                        ILab lab = new Lab3();
                        ExecuteLab(lab, input, output);
                    });
                });
            });

            app.Command("set-path", configCmd =>
            {
                var path = configCmd.Option("-p|--path", "set path", CommandOptionType.SingleOrNoValue);

                configCmd.OnExecute(() =>
                {
                    if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config")))
                    {
                        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config"));
                    }

                    if (path.HasValue())
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config", "my-tool.cfg"), $"LAB_PATH={path.Value() + Environment.NewLine}");
                        }
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config", "my-tool.cfg"), $"LAB_PATH {path.Value()}");
                        }
                    }
                    else
                    {
                        File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config", "my-tool.cfg"));
                    }
                });
            });

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }
    }
}
