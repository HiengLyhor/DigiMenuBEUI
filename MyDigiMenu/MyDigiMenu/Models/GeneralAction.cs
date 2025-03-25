using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyDigiMenu.Models
{
    public class GeneralAction
    {
        private const string BotToken = "8096834023:AAFPb86tmooUK5IvhWK5UAzZMueUhx8IFyw";  // @emenu_alert_bot
        private const string ChatId = "-1002542448425"; // https://t.me/+pR1IZRLvI3s4ZTZl E-Menu Alert

        public static async Task SendMessageAsync(string message)
        {
            // Define the Telegram API URL
            var url = $"https://api.telegram.org/bot{BotToken}/sendMessage";

            // Prepare the message content
            var data = new
            {
                chat_id = ChatId,
                text = message
            };

            using (var client = new HttpClient())
            {
                // Serialize the data object into a JSON string
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                try
                {
                    // Send the HTTP request to Telegram's API
                    var response = await client.PostAsync(url, content);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }

        public static async Task<T> GetAsync<T>(string url, string jwtToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(jsonString);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
        }

        public static async Task<T> PostAsync<T>(string url, object data, string jwtToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(jsonString);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
        }

        public static string GetBaseAPIUrl()
        {
            return ConfigurationManager.AppSettings["BaseApiUrl"];
        }
    }
}