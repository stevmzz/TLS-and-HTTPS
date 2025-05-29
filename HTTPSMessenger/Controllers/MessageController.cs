using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Text;

namespace HTTPSMessenger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        // endpoint para procesar y retransmitir mensajes via protocolo tls
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            try
            {
                Console.WriteLine($"Servidor HTTPS ha recibido mensaje: {request.Message}");

                // retransmitir mensaje al servicio tls de destino
                string tlsResponse = await SendToTLS(request.Message);

                return Ok(new
                {
                    Status = "Mensaje procesado y retransmitido exitosamente",
                    OriginalMessage = request.Message,
                    SentVia = "Protocolo HTTPS con cifrado TLS",
                    TLSResponse = tlsResponse,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante procesamiento del mensaje: {ex.Message}");
            }
        }



        // endpoint para consultar estado operacional del servicio de retransmision
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                Service = "Servicio de Retransmision HTTPS-TLS",
                Status = "Sistema operacional y disponible para retransmision",
                Protocol = "HTTPS con soporte de retransmision TLS",
                Port = 7154,
                TLSTarget = "Servicio TLS en localhost:8080",
                Timestamp = DateTime.Now
            });
        }



        // establece conexion tcp y retransmite mensaje al servicio tls
        private async Task<string> SendToTLS(string message)
        {
            try
            {
                // establecer conexion tcp con el servicio tls de destino
                using var client = new TcpClient();
                await client.ConnectAsync("localhost", 8080);
                using var stream = client.GetStream();

                // formatear mensaje con protocolo personalizado para tls
                string tlsMessage = $"HTTPS_MSG:{message}";
                byte[] data = Encoding.UTF8.GetBytes(tlsMessage);
                await stream.WriteAsync(data, 0, data.Length);

                Console.WriteLine($"Mensaje retransmitido al servicio TLS: {tlsMessage}");

                // recibir confirmacion de procesamiento del servicio tls
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Confirmacion recibida del servicio TLS: {response}");
                return response;
            }
            catch (Exception ex)
            {
                return $"Error estableciendo comunicacion con servicio TLS: {ex.Message}";
            }
        }
    }



    // modelo para estructurar peticiones de mensajeria entrantes
    public class MessageRequest
    {
        public string Message { get; set; } = ""; // contenido del mensaje a retransmitir
        public string From { get; set; } = "Cliente HTTPS Corporativo"; // identificacion del remitente
    }
}