using Compilation_Service.Dto.Response;
using Compilation_Service.RabbitMQ;
using Compilation_Service.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Seq("http://host.docker.internal:5341/")
    .CreateLogger();

// Register services
builder.Services.AddScoped<MemoryMonitorService>();
builder.Services.AddSingleton<RabbitMqService>(); 
builder.Services.AddSingleton<RequestListener>();
builder.Services.AddScoped<CppTestingService>();
builder.Services.AddScoped<PythonTestingService>();
builder.Services.AddScoped<SubmissionRequestHandler>();

var app = builder.Build();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var rabbitMqService = services.GetRequiredService<RabbitMqService>();
    var requestListener = new RequestListener(rabbitMqService, services.GetRequiredService<IServiceScopeFactory>(), services.GetRequiredService<ILogger<RequestListener>>());
}

app.Run();
