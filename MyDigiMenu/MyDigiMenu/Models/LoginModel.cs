using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MyDigiMenu.Models
{
	public class LoginModel
	{
		public string Username { get; set; }
		public string Password { get; set; }

        private static readonly HttpClient client = new HttpClient();

        public async Task<LoginResponse> loginUserToAPI(LoginModel loginUser)
        {

			try
			{
				string baseUrl = ConfigurationManager.AppSettings["BaseApiUrl"];

                var jsonContent = JsonConvert.SerializeObject(loginUser);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUrl + "user/login", content);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

                return loginResponse;

			}
			catch (Exception ex)
			{
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

	}
}