using System;
using System.IO;
using CommandLine;

namespace SelfExtractingZip.Builder
{

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts =>
                {
                    if (!File.Exists(opts.StubPath))
                    {
                        Console.Error.WriteLine($"Stub EXE not found: {opts.StubPath}");
                        return;
                    }
                    if (!File.Exists(opts.ZipPath))
                    {
                        Console.Error.WriteLine($"ZIP file not found: {opts.ZipPath}");
                        return;
                    }

                    using var outStream = new FileStream(opts.OutputPath, FileMode.Create, FileAccess.Write);
                    using var stubStream = new FileStream(opts.StubPath, FileMode.Open, FileAccess.Read);
                    stubStream.CopyTo(outStream);
                    using var zipStream = new FileStream(opts.ZipPath, FileMode.Open, FileAccess.Read);
                    zipStream.CopyTo(outStream);

                    Console.WriteLine($"Self-extracting EXE created: {opts.OutputPath}");
                });
        }
    }
}