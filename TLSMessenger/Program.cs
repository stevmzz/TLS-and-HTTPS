using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TLSMessenger
{
    class Program
    {
        // contador global de mensajes procesados por el servicio
        private static int messageCount = 0;

        // punto de entrada del servidor de mensajeria tls
        static async Task Main(string[] args)
        {
            Console.WriteLine("SERVICIO DE MENSAJERIA TLS");
            Console.WriteLine("Puerto de escucha: 8080");
            Console.WriteLine("Esperando conexiones entrantes del servicio HTTPS de retransmision...");
            Console.WriteLine("Protocolo: Comunicacion TCP directa simulando capa TLS\n");

            // inicializar listener tcp en puerto designado
            var listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            while (true)
            {
                try
                {
                    // aceptar conexion entrante de forma asincrona
                    var client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine($"Nueva conexion establecida - Sesion #{++messageCount} desde servicio HTTPS");

                    // procesar cliente en hilo separado para concurrencia
                    _ = Task.Run(() => HandleClient(client));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en listener principal: {ex.Message}");
                }
            }
        }



        // procesa la comunicacion con un cliente individual
        static async Task HandleClient(TcpClient client)
        {
            try
            {
                using var stream = client.GetStream();

                // recibir datos del cliente mediante stream tcp
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Mensaje recibido en sesion: {receivedMessage}");

                // procesar mensaje segun protocolo establecido
                string response = ProcessMessage(receivedMessage);

                // transmitir respuesta de confirmacion al cliente
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

                Console.WriteLine($"Confirmacion enviada al cliente: {response}");
                Console.WriteLine($"Estadisticas del servicio - Total de mensajes procesados: {messageCount}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante procesamiento de sesion cliente: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }



        // analiza y procesa el contenido del mensaje recibido
        static string ProcessMessage(string message)
        {
            // verificar formato de protocolo https-tls
            if (message.StartsWith("HTTPS_MSG:"))
            {
                // extraer contenido del mensaje
                string content = message.Substring("HTTPS_MSG:".Length);
                var timestamp = DateTime.Now.ToString("HH:mm:ss");

                // generar respuesta estructurada con metadatos
                return $"Mensaje procesado exitosamente: '{content}' | Procesado: {timestamp} | Protocolo: TCP/TLS | Sesion: {messageCount}";
            }

            return "Error: Formato de mensaje no compatible con protocolo TLS establecido";
        }
    }
}