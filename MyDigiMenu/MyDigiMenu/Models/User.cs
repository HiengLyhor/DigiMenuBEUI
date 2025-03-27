using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace MyDigiMenu.Models
{
    public class User
    {

        public string Username { get; set; }
        public string OwnShop { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpDate { get; set; }
        public string IsSpecial { get; set; }
        public string IsLock { get; set; }

        public async Task<AllUserResponse> GetAllUsers(string filterBy, string asc, string jwtToken, string user)
        {
            try
            {

                var requestBody = new
                {
                    FilterBy = filterBy,
                    Asc = asc,
                    UserRequest = user
                };

                var url = GeneralAction.GetBaseAPIUrl() + "user/all-user";
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
    public class AllUserResponse : User
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public AllUserResponse(int Code, string message)
        {
            this.Code = Code;
            this.Message = message;
        }

        public List<User> UserLogins { get; set; }

        public AllUserResponse() { }
    }
}