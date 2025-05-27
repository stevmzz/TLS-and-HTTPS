var builder = WebApplication.CreateBuilder(args);

// configurar servicios de controladores para manejo de endpoints
builder.Services.AddControllers();

// habilitar exploracion de endpoints para documentacion automatica
builder.Services.AddEndpointsApiExplorer();

// configurar generacion de documentacion swagger con metadatos personalizados
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Secure Message API - Demonstracion HTTPS",
        Description = "API que demuestra implementacion y beneficios de comunicacion segura HTTPS",
        Version = "v1.0"
    });
});

var app = builder.Build();

// configurar pipeline de middleware para entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    // habilitar generacion de especificacion swagger
    app.UseSwagger();

    // configurar interfaz web de swagger como pagina principal
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Secure Message API v1");
        c.RoutePrefix = ""; // establecer swagger ui como pagina de inicio
        c.DocumentTitle = "Secure Message API - Documentacion";
    });
}

// forzar redireccion automatica de http a https para mayor seguridad
app.UseHttpsRedirection();

// habilitar middleware de autorizacion para control de acceso
app.UseAuthorization();

// mapear controladores a rutas automaticamente
app.MapControllers();

// iniciar servidor web y escuchar peticiones entrantes
app.Run();