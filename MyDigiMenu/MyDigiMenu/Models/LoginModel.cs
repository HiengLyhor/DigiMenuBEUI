using Newtonsoft.Json;
using System;
using System.Net;
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

				if (loginResponse.Code.Equals(((int)HttpStatusCode.InternalServerError)))
				{
                    await GeneralAction.SendMessageAsync("#LoginModel_Error\n#Error: " + loginResponse.Message + "\nError At: (UTC) " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    return new LoginResponse { Code = (int)HttpStatusCode.InternalServerError, Message = "Error occurred during login." };
                }

				loginResponse.Token = Encryption.Decrypt(loginResponse.Token); // Will catch if no token provided

                return loginResponse;

			}
			catch (Exception ex)
			{
				await GeneralAction.SendMessageAsync("#LoginModel_Error\n#Error: " + ex.Message + "\nError At: (UTC) " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                return new LoginResponse { Code = (int)HttpStatusCode.InternalServerError, Message = "Error occurred during login." };
            }

        }

    }
	public class LoginResponse 
	{
		public int Code { get; set; }

		public string Message { get; set; }

		public string Username {  set; get; }

		public DateTime? CreateDate { get; set; }

		public DateTime? ExpDate { get; set; }
		
		public bool? Active { get; set; }

		public string Role { get; set; }

		public string Token { get; set; }

		public string ShopName { get; set; }
	}
}