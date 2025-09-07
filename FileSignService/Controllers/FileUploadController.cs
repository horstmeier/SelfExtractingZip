using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly SignQueue _signQueue;

    public FileUploadController(SignQueue signQueue)
    {
        _signQueue = signQueue;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(long.MaxValue)]
    public async Task<IActionResult> Upload()
    {
        var id = Guid.NewGuid().ToString();
        var filePath = Path.Combine("uploads", $"{id}.bin");
        Directory.CreateDirectory("uploads");

        await using var stream = new FileStream(filePath, FileMode.Create);
        await Request.Body.CopyToAsync(stream);

        _signQueue.Enqueue(filePath, id);

        return Ok(new { id });
    }

    [HttpGet("status/{id}")]
    public IActionResult Status(string id)
    {
        var status = _signQueue.GetStatus(id);
        return Ok(new { id, status });
    }
}