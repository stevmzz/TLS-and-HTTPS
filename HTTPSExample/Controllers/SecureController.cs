using Microsoft.AspNetCore.Mvc;

namespace HTTPSWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        // endpoint para verificar el estado de seguridad de la conexion actual
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                IsSecure = Request.IsHttps,
                Message = Request.IsHttps ? "Conexion HTTPS activa y segura" : "Advertencia: conexion HTTP no cifrada",
                Protocol = Request.Scheme,
                Host = Request.Host.Value,
                Timestamp = DateTime.Now
            });
        }



        // endpoint para recibir mensajes de forma segura usando https obligatorio
        [HttpPost("message")]
        [RequireHttps]
        public IActionResult SendMessage([FromBody] SimpleMessage message)
        {
            if (string.IsNullOrEmpty(message?.Content))
                return BadRequest("El contenido del mensaje no puede estar vacio");

            return Ok(new
            {
                Status = "Mensaje recibido y procesado de forma segura",
                From = message.From,
                Content = message.Content,
                Security = "Datos protegidos mediante cifrado HTTPS/TLS",
                ProcessedAt = DateTime.Now
            });
        }



        // endpoint que demuestra las ventajas de https vs http
        [HttpGet("demo")]
        public IActionResult GetDemo()
        {
            return Ok(new
            {
                HTTPSBenefits = new[] {
                    "Cifrado completo de datos en transito",
                    "Autenticacion verificada del servidor",
                    "Garantia de integridad de los datos"
                },
                HTTPRisks = new[] {
                    "Transmision de datos en texto plano",
                    "Vulnerabilidad a ataques de interceptacion",
                    "Posibilidad de modificacion de datos"
                },
                CurrentConnection = Request.IsHttps ? "Conexion segura establecida" : "Conexion no segura detectada"
            });
        }
    }



    // modelo para representar mensajes simples en el sistema
    public class SimpleMessage
    {
        public string From { get; set; } = ""; // remitente del mensaje
        public string Content { get; set; } = ""; // contenido del mensaje
    }
}