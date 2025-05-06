using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MyDigiMenu.Models
{
    public class User
    {

        public string Username { get; set; }
        public string ShopName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpDate { get; set; }
        public string Role { get; set; }
        public bool Active{ get; set; }

        public async Task<AllUserResponse> GetAllUsers(string filterBy, string jwtToken, string user)
        {
            try
            {

                var requestBody = new
                {
                    SearchVal = filterBy,
                    UserRequest = user
                };

                var url = GeneralAction.GetBaseAPIUrl() + "user/all";
                var result = await GeneralAction.PostAsync<AllUserResponse>(url, requestBody, jwtToken);

                return result;

            }
            catch (Exception ex)
            {
                await GeneralAction.SendMessageAsync("Error occurred during GetAllUsers: " + ex.Message);
                return new AllUserResponse((int)HttpStatusCode.InternalServerError, "Error occurred during GetAllUsers.");
            }

        }
    }
    public class AllUserResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public AllUserResponse(int Code, string message)
        {
            this.Code = Code;
            this.Message = message;
        }

        public List<User> UserInfo { get; set; }

        public AllUserResponse() { }
    }
}