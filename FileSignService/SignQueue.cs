using System.Collections.Concurrent;

public class SignQueue
{
    private readonly ConcurrentQueue<(string filePath, string id)> _queue = new();
    private readonly ConcurrentDictionary<string, string> _status = new();

    public void Enqueue(string filePath, string id)
    {
        _status[id] = "Queued";
        _queue.Enqueue((filePath, id));
    }

    public string GetStatus(string id) => _status.TryGetValue(id, out var status) ? status : "Unknown";

    public void SetStatus(string id, string status) => _status[id] = status;

    public bool TryDequeue(out (string filePath, string id) item) => _queue.TryDequeue(out item);
}