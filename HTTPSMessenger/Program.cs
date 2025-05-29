var builder = WebApplication.CreateBuilder(args);

// configurar servicios de controladores para endpoints de retransmision
builder.Services.AddControllers();

// habilitar exploracion automatica de endpoints para documentacion
builder.Services.AddEndpointsApiExplorer();

// configurar documentacion swagger para el servicio de retransmision
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Servicio de Retransmision HTTPS-TLS",
        Description = "Servidor intermediario que recibe mensajes via HTTPS y los retransmite mediante comunicacion TLS directa a servicios de destino",
        Version = "v1.0.0",
        Contact = new()
        {
            Name = "Equipo de Infraestructura de Comunicaciones",
            Email = "infraestructura@empresa.com"
        }
    });
});

var app = builder.Build();

// configurar pipeline de middleware para entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    // activar generacion de especificacion openapi
    app.UseSwagger();

    // configurar interfaz interactiva de documentacion
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = ""; // establecer como pagina principal
        c.DocumentTitle = "Servicio de Retransmision HTTPS-TLS - Documentacion";
        c.DisplayRequestDuration(); // mostrar tiempos de respuesta
    });
}

// aplicar redireccion automatica a https para seguridad
app.UseHttpsRedirection();

// mapear controladores a rutas automaticamente
app.MapControllers();

// mostrar informacion de inicializacion del servicio
Console.WriteLine("Servicio de Retransmision HTTPS-TLS iniciado correctamente");
Console.WriteLine("Servidor HTTPS operacional en: https://localhost:7154");
Console.WriteLine("Destino de retransmision TLS configurado: localhost:8080");
Console.WriteLine("Estado: Sistema listo para procesar retransmisiones de mensajes");

// iniciar servidor y mantener escucha activa
app.Run();