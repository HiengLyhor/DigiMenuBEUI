using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyDigiMenu.Models
{
	public class LoginModel
	{
		public string Username { get; set; }
		public string Password { get; set; }

        private static readonly HttpClient client = new HttpClient();

        public async Task<LoginResponse> LoginUserToAPI(LoginModel loginUser)
        {

			try
			{

                loginUser.Password = Encryption.Encrypt(loginUser.Password);

                var jsonContent = JsonConvert.SerializeObject(loginUser);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GeneralAction.GetBaseAPIUrl() + "user/login", content);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

				loginResponse.Token = Encryption.Decrypt(loginResponse.Token);

                return loginResponse;

			}
			catch (Exception ex)
			{
				if (ex.InnerException.Message.Equals("Unable to connect to the remote server")) await GeneralAction.SendMessageAsync($"Backend Web Unable to connect to API.\nError At: (UTC) {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}"); // Push Tele Notif
                return new LoginResponse { Code = 500, Message = "Error occurred during login." };
            }

        }

    }
	public class LoginResponse 
	{
		public int Code { get; set; }

		public string Message { get; set; }

		public string Username {  set; get; }

		public DateTime CreateDate { get; set; }

		public DateTime ExpDate { get; set; }

		public string IsSpecial { get; set; }
		
		public string IsLock { get; set; }

		public string Token { get; set; }

		public string VenueName { get; set; }

		public string ShopName { get; set; }
	}
}