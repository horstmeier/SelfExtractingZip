using CommandLine;

namespace SelfExtractingZip.Builder
{
    class Options
    {
        [Option("zip", Required = true, HelpText = "Path to the ZIP file.")]
        public string ZipPath { get; set; }

        [Option("output", Required = true, HelpText = "Path to the output EXE.")]
        public string OutputPath { get; set; }

        [Option("stub", Required = false, HelpText = "Path to the stub EXE.", Default = "Stub/bin/Release/net6.0/Stub.exe")]
        public string StubPath { get; set; }
    }
}