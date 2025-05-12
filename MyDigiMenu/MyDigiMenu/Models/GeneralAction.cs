using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
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

                var jsonContent = JsonConvert.SerializeObject(data);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
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

        public List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }

                list.Add(dict);
            }

            return list;
        }

        public static string GetBaseAPIUrl()
        {
            return ConfigurationManager.AppSettings["BaseApiUrl"];
        }

        public static string GetAdminRight()
        {
            return Encryption.Encrypt(ConfigurationManager.AppSettings["AdminRights"]);
        }

        public static string CompressAndConvertToBase64(HttpPostedFileBase file)
        {
            using (var image = Image.FromStream(file.InputStream))
            {
                // Step 1: Compress the image by 50%
                int newWidth = (int)(image.Width * 0.3);
                int newHeight = (int)(image.Height * 0.3);
                var compressedImage = new Bitmap(image, newWidth, newHeight);

                // Step 2: Convert the compressed image to a memory stream
                using (var memoryStream = new MemoryStream())
                {
                    compressedImage.Save(memoryStream, ImageFormat.Png); // Save as PNG format
                    byte[] imageBytes = memoryStream.ToArray();

                    // Step 3: Convert the byte array to a Base64 string
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }
    }
}