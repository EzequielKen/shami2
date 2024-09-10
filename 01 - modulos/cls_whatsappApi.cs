using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _01___modulos
{
    public class cls_whatsappApi
    {
        private readonly string _whatsappBusinessToken = "EAAYG1LCD4lsBO9KM02k3jBnGjnetRCKMedvWeIpog3OHNPXhHXjPRjK9s4wwUFCWEh2fvusRAKPk3YGkT94A3G6quvZBO7QQtAZCTZBBZCf5rNoB7HMLJZAiNSZA4GZA44yM5B7pVPkXwlNWmBqrbN3kUfbnpMAepZBhMekmM4S41iP5oYpGwHeIf0LPGU1JZChcMTwZDZD";
        private readonly string _whatsappPhoneNumberId = "5491134060253";
        private readonly string _apiUrl = "https://graph.facebook.com/v17.0/";

        /// <summary>
        /// Envía un mensaje de texto a través de la API de WhatsApp.
        /// </summary>
        /// <param name="numero">Número de WhatsApp del destinatario (formato internacional con código de país).</param>
        /// <param name="mensaje">Texto del mensaje a enviar.</param>
        public void EnviarMensajeWhatsApp(string numero, string mensaje)
        {
            using (var httpClient = new HttpClient())
            {
                // Configuración del encabezado de autorización
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _whatsappBusinessToken);

                // Crear el objeto con el contenido del mensaje
                var data = new
                {
                    messaging_product = "whatsapp",
                    to = numero,
                    type = "text",
                    text = new { body = mensaje }
                };

                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API de WhatsApp
                var response = httpClient.PostAsync($"{_apiUrl}{_whatsappPhoneNumberId}/messages", content).Result;

                // Verificar la respuesta
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mensaje enviado con éxito.");
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Error al enviar mensaje: {response.StatusCode}. Detalles: {errorMessage}");
                }
            }
        }
    }
}
