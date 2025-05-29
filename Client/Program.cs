using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Client
{
    class Program
    {
        // cliente http persistente para comunicaciones con el servidor https
        private static HttpClient httpClient = new HttpClient();

        // punto de entrada del cliente de mensajeria https-tls
        static async Task Main(string[] args)
        {
            // configurar cliente http para entornos de desarrollo
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            httpClient = new HttpClient(handler);

            Console.WriteLine("SISTEMA DE MENSAJERIA SEGURA - CLIENTE HTTPS");
            Console.WriteLine("Inicializando conexion con servidor de retransmision HTTPS-TLS\n");

            while (true)
            {
                ShowMenu();
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await SendMessage();
                        break;
                    case "2":
                        await CheckStatus();
                        break;
                    case "3":
                        Console.WriteLine("Cerrando sesion de mensajeria. Conexion terminada.");
                        return;
                    default:
                        Console.WriteLine("Error: Codigo de operacion no reconocido. Intente nuevamente.");
                        break;
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar con la sesion...");
                Console.ReadKey();
                Console.Clear();
            }
        }



        // presenta opciones de comunicacion disponibles en el sistema
        static void ShowMenu()
        {
            Console.WriteLine("SISTEMA DE COMUNICACION HTTPS-TLS - MENU PRINCIPAL");
            Console.WriteLine("1. Transmision de mensaje via protocolo hibrido HTTPS-TLS");
            Console.WriteLine("2. Consultar estado operacional del sistema servidor");
            Console.WriteLine("3. Finalizar sesion de cliente");
            Console.Write("\nSeleccione operacion deseada: ");
        }



        // envia mensaje al servidor para retransmision via tls
        static async Task SendMessage()
        {
            try
            {
                Console.Write("\nIngrese contenido del mensaje a transmitir: ");
                string message = Console.ReadLine() ?? "";

                if (string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Error: El contenido del mensaje no puede estar vacio");
                    return;
                }

                // estructurar peticion para el servidor de retransmision
                var request = new { Message = message, From = "Cliente HTTPS Corporativo" };
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine("\nInicializando transmision via protocolo HTTPS seguro...");
                Console.WriteLine("Redirigiendo mensaje a servicio de retransmision TLS...");

                // enviar mensaje al servidor intermediario https
                var response = await httpClient.PostAsync("https://localhost:7154/api/message/send", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // procesar confirmacion de entrega del sistema
                    dynamic result = JsonConvert.DeserializeObject(responseContent);

                    Console.WriteLine("\nOperacion de mensajeria completada exitosamente:");
                    Console.WriteLine($"Mensaje transmitido: {result.originalMessage}");
                    Console.WriteLine($"Protocolo de envio: {result.sentVia}");
                    Console.WriteLine($"Confirmacion del destino TLS: {result.tlsResponse}");
                    Console.WriteLine($"Marca temporal del procesamiento: {result.timestamp}");

                    Console.WriteLine("\nFlujo de comunicacion establecido:");
                    Console.WriteLine("   Cliente HTTPS → Servidor Intermediario → Servicio TLS → Confirmacion de Entrega");
                }
                else
                {
                    Console.WriteLine($"Error del servidor: Codigo de respuesta {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conectividad: {ex.Message}");
            }
        }



        // verifica el estado operacional del sistema de retransmision
        static async Task CheckStatus()
        {
            try
            {
                Console.WriteLine("\nConsultando estado operacional del sistema de retransmision...");

                // solicitar informacion de estado del servidor
                var response = await httpClient.GetAsync("https://localhost:7154/api/message/status");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // analizar informacion de estado recibida
                    dynamic status = JsonConvert.DeserializeObject(content);

                    Console.WriteLine("\nInforme del estado del sistema:");
                    Console.WriteLine($"Identificacion del servicio: {status.service}");
                    Console.WriteLine($"Estado operacional: {status.status}");
                    Console.WriteLine($"Protocolo de comunicacion: {status.protocol}");
                    Console.WriteLine($"Puerto de escucha HTTPS: {status.port}");
                    Console.WriteLine($"Servicio TLS de destino: {status.tlsTarget}");
                    Console.WriteLine($"Ultima actualizacion: {status.timestamp}");
                }
                else
                {
                    Console.WriteLine($"Error consultando estado: Codigo de respuesta {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de comunicacion: {ex.Message}");
            }
        }
    }
}