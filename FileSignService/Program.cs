using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<SignQueue>();
builder.Services.AddHostedService<BackgroundSignService>();

var app = builder.Build();
app.MapControllers();
app.Run();