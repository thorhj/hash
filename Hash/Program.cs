using CommandLine;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Hash
{
    class Program
    {
        static int Main(string[] args)
        {
            var runner = new Runner();
            return runner.Run(args);
        }
    }

    public class Runner
    {
        public int Run(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<Sha1Options/*, Md5Options*/>(args)
                .MapResult(
                    (Sha1Options opts) => RunSha1(opts),
                    //(Md5Options opts) => 0,
                    errs => 1);
        }

        private void EnsureInput(HashOptions options)
        {
            if (!options.Secret)
            {
                while (string.IsNullOrEmpty(options.Input))
                {
                    Console.Write("Enter text: ");
                    options.Input = Console.ReadLine();
                }
            }
            if (options.Secret)
            {
                while (string.IsNullOrEmpty(options.Input))
                {
                    Console.Write("Enter text: ");
                    var builder = new StringBuilder();
                    while (true)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                        else if (key.Key == ConsoleKey.Backspace)
                        {
                            builder.Remove(builder.Length - 1, 1);
                        }
                        else
                        {
                            builder.Append(key.KeyChar);
                        }
                    }
                    options.Input = builder.ToString();
                    Console.WriteLine();
                }
            }
        }

        private int RunSha1(Sha1Options options)
        {
            EnsureInput(options);
            Console.WriteLine(options.Checksum);
            Console.WriteLine(options.Secret);
            Console.WriteLine(options.Input);
            return 0;
        }
    }

    public abstract class HashHandler<TAlgorithm, TOptions> where TAlgorithm : HashAlgorithm where TOptions : class
    {
        protected TAlgorithm HashAlgorithm;

        public void MasterOfDisaster(byte[] input)
        {
            HashAlgorithm.ComputeHash(input);
        }
    }

    [Verb("sha1")]
    public class Sha1Options : HashOptions
    {
    }

    [Verb("md5")]
    public class Md5Options : HashOptions
    {
    }

    public class HashOptions
    {
        [Option('s', "secret", 
            Required = false, 
            HelpText = "Instead of entering the input text directly in the command, this options lets you enter the input to a prompt that will not be written to the console.")]
        public bool Secret { get; set; }

        [Option('c', "checksum",
            Required = false, 
            HelpText = "Compute a checksum of a file instead of hashing the input string. The input should be a path to a file or directory. If the path represents a directory, the tool will compute the checksum for files in that folder.")]
        public bool Checksum { get; set; }

        [Value(0, 
            MetaName = "Input text",
            HelpText = "The input text to hash. If the --checksum flag is set, this will interpreted as a path to a file or directory instead. If the --secret flag is set, this value will be ignored, and the input text will be collected from the subsequent prompt.")]
        public string Input { get; set; }
    }
}
