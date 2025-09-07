using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace SelfExtractingZip.Stub;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            bool verbose = args.Length > 0 && args[0] == "-v";
            int pathArgIndex = verbose ? 1 : 0;

            var executableName = Assembly.GetEntryAssembly().GetName().Name;
            var exePath = Path.Combine(AppContext.BaseDirectory, executableName + ".exe");
            using var exeStream = new FileStream(exePath, FileMode.Open, FileAccess.Read);
            var zipStart = FindZipStart(exeStream);
            if (zipStart < 0)
            {
                Console.Error.WriteLine("ZIP archive not found in EXE.");
                Environment.Exit(1);
            }
            exeStream.Seek(zipStart, SeekOrigin.Begin);
            using var zipArchive = new ZipArchive(exeStream, ZipArchiveMode.Read);
            var extractPath = args.Length > pathArgIndex
                ? args[pathArgIndex]
                : Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(extractPath);
            zipArchive.ExtractToDirectory(extractPath);
            // Print extraction path if verbose or no path argument provided
            if (verbose || args.Length <= pathArgIndex)
            {
                Console.WriteLine($"Extracted to: {extractPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }

    static long FindZipStart(Stream exeStream)
    {
        // ZIP files start with "PK\x03\x04"
        var signature = new byte[] { (byte)'P', (byte)'K', 3, 4 };
        var buffer = new byte[4];
        exeStream.Seek(0, SeekOrigin.Begin);
        for (var i = 0L; i < exeStream.Length - 4; i++)
        {
            var bytesRead = exeStream.Read(buffer, 0, 4);
            if (bytesRead < 4) break;
            if (buffer[0] == signature[0] &&
                buffer[1] == signature[1] &&
                buffer[2] == signature[2] &&
                buffer[3] == signature[3])
            {
                return i;
            }
            exeStream.Seek(i + 1, SeekOrigin.Begin);
        }
        return -1;
    }
}