using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Persistence;
using ProjectManagement.Application;
using ProjectManagement.Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Los logs se guardan en la carpeta logs de la raíz del repositorio.
// Se utiliza la ruta de la solución para no depender del directorio desde el que se inicie la aplicación.
var projectRoot = FindProjectRoot(builder.Environment.ContentRootPath);

var logsDirectory = Path.Combine(projectRoot, "logs");
Directory.CreateDirectory(logsDirectory);

var logFilePath = Path.Combine(logsDirectory, "projectmanagement-.log");

Console.WriteLine($"Directorio de logs: {logsDirectory}");
Console.WriteLine($"Archivo de logs: {logFilePath}");


// Registra los eventos tanto en consola como en archivos diarios.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        logFilePath,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,
        shared: true)
    .CreateLogger();

try
{
    Log.Information("Iniciando Project Management API");

    // Integra Serilog con el sistema de logging de ASP.NET Core.
    builder.Host.UseSerilog();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Centraliza el tratamiento de excepciones y su respuesta HTTP.
    app.UseSerilogRequestLogging();
    app.UseMiddleware<GlobalExceptionMiddleware>();

    // Aplica las migraciones pendientes al iniciar la aplicación.
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();

        Log.Information("Migraciones de base de datos aplicadas correctamente");
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("Project Management API preparada para recibir peticiones");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación ha terminado inesperadamente");
}
finally
{
    // Vacía los buffers y libera los recursos del sistema de logging.
    await Log.CloseAndFlushAsync();
}


static string FindProjectRoot(string contentRootPath)
{
    // Busca la raíz de la solución para evitar depender del directorio
    // de trabajo desde el que se inicia la API.
    var directory = new DirectoryInfo(contentRootPath);

    while (directory is not null)
    {
        if (File.Exists(Path.Combine(directory.FullName, "ProjectManagement.slnx")))
        {
            return directory.FullName;
        }

        directory = directory.Parent;
    }

    // Si se ejecuta una publicación sin la solución, usa el directorio
    // de la aplicación como ubicación alternativa.
    return AppContext.BaseDirectory;
}
