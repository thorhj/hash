using CommandLine;
using System;
using System.Security.Cryptography;

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
            return CommandLine.Parser.Default.ParseArguments<Sha1Options>(args)
              .MapResult(
                (Sha1Options opts) => RunSha1(opts),
                errs => 1);
        }

        public int RunSha1(Sha1Options options)
        {
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

    public class HashOptions
    {
        [Option('s', "secret", Required = false, HelpText = "Instead of entering the input text directly in the command, this options lets you enter the input to a prompt that will not be written to the console.")]
        public bool Secret { get; set; }

        [Option('c', "checksum", Required = false, HelpText = "Compute a checksum of a file instead of hashing the input string. The input should be a path to a file or directory. If the path represents a directory, the tool will compute the checksum for files in that folder.")]
        public bool Checksum { get; set; }

        [Value(0)]
        public string Input { get; set; }
    }
}
