# SelfExtractingZip

A cross-platform tool written in C# for creating self-extracting archives.  
It attaches a minimal extractor stub to a ZIP file, producing an EXE that extracts its contents when run.

## Features

- **Minimal EXE stub:** No GUI, simply extracts the embedded ZIP file.
- **Builder tool:** Combines a ZIP file and the stub to make a self-extracting EXE.
- **Cross-platform:** Works on Windows, Linux, and MacOS via .NET.

## Structure

- `/Stub/` — Minimal extractor stub (C#)
- `/Builder/` — Builder tool (C#)
- `/README.md` — This documentation

## Usage

1. Build the `Stub` project (single-file publish recommended).
2. Use the `Builder` tool to combine any ZIP file with the stub, creating a self-extracting EXE:
   ```
   dotnet run --project Builder -- --zip myarchive.zip --output myextractor.exe
   ```
3. Run `myextractor.exe` to extract contents.

## How it works

- The `Builder` tool appends the ZIP file to the stub EXE.
- The `Stub` locates and extracts the appended ZIP when executed.

## Requirements

- [.NET 6.0 or newer](https://dotnet.microsoft.com/download)

## TODO

- [ ] Minimal stub implementation
- [ ] Builder tool implementation
- [ ] CLI options for extraction location
- [ ] Tests and examples

## License

MIT (or your preferred license)