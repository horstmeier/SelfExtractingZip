using Microsoft.Extensions.Hosting;
using System.Diagnostics;

public class BackgroundSignService : BackgroundService
{
    private readonly SignQueue _signQueue;

    public BackgroundSignService(SignQueue signQueue)
    {
        _signQueue = signQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_signQueue.TryDequeue(out var item))
            {
                _signQueue.SetStatus(item.id, "Signing");
                // Replace with your actual signtool command and cert args
                var psi = new ProcessStartInfo("signtool.exe", $"sign /fd SHA256 /a \"{item.filePath}\"")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                var process = Process.Start(psi);
                await process.WaitForExitAsync(stoppingToken);

                _signQueue.SetStatus(item.id, process.ExitCode == 0 ? "Signed" : "Failed");
            }
            else
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}