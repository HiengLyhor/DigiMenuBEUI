using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace MyDigiMenu.Models
{
    public class User
    {

        public int Code { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ShopName { get; set; }
        public string ShopKey { get; set; }
        public DateTime? ExpDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Active { get; set; }
        public string Role { get; set; }
        public string ShopDesc { get; set; }
        public string ShopLocation { get; set; }
        public string OpenCloseTime { get; set; }
        public string ImgName { get; set; }
        public HttpPostedFileBase NewImage { get; set; }
        public string TelegramId { get; set; }
        public string Social { get; set; }

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
        public DataSet ConvertAllUserResponseToDataSet(AllUserResponse response)
        {
            DataSet ds = new DataSet();

            // Create main data table
            DataTable userTable = new DataTable("UserInfo");

            // Define columns (adjust based on your needs)
            userTable.Columns.Add("Username", typeof(string));
            userTable.Columns.Add("ShopName", typeof(string));
            userTable.Columns.Add("ShopKey", typeof(string));
            userTable.Columns.Add("Role", typeof(string));
            userTable.Columns.Add("Active", typeof(bool));
            userTable.Columns.Add("ExpDate", typeof(DateTime));
            userTable.Columns.Add("CreateDate", typeof(DateTime));
            userTable.Columns.Add("TelegramId", typeof(string));
            userTable.Columns.Add("Social", typeof(string));

            // Populate data
            foreach (var user in response.UserInfo)
            {
                userTable.Rows.Add(
                    user.Username,
                    user.ShopName,
                    user.ShopKey,
                    user.Role,
                    user.Active,
                    user.ExpDate,
                    user.CreateDate,
                    user.TelegramId,
                    user.Social
                );
            }

            // Add count table (if you need separate count)
            DataTable countTable = new DataTable("Counts");
            countTable.Columns.Add("TotalCount", typeof(int));
            countTable.Rows.Add(response.UserInfo.Count);

            ds.Tables.Add(userTable);
            ds.Tables.Add(countTable);

            return ds;
        }

        internal async Task<StatusResponse> CreateUser(UserInfoRequestToAPI request, string token)
        {
            try
            {

                var url = GeneralAction.GetBaseAPIUrl() + "user/register";

                var requestBody = new
                {
                    SpecialKey = request.SpecialKey,
                    Username = request.Username,
                    Password = request.Password,
                    Role = request.Role,
                    ShopName = request.ShopName,
                    TelegramId = request.TelegramId,
                    ShopDesc = request.ShopDesc,
                    ShopLocation = request.ShopLocation,
                    OpenCloseTime = request.OpenCloseTime,
                    ShopImg = request.ShopImg,
                    Social = request.Social
                };

                var result = await GeneralAction.PostAsync<StatusResponse>(url, requestBody, token);

                return result;

            }
            catch (Exception ex)
            {
                return new StatusResponse { Code = 500, Message = ex.Message };
            }
        }

        internal async Task<StatusResponse> DisableUser(UsernameAndRequester request, string token)
        {
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "user/disable";

                var result = await GeneralAction.PostAsync<StatusResponse>(url, request, token);

                return result;
            }
            catch (Exception ex)
            {
                return new StatusResponse { Code = 500, Message = ex.Message };
            }
        }

        internal async Task<StatusResponse> EnableUser(UsernameAndRequester request, string token)
        {
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "user/enable";

                var result = await GeneralAction.PostAsync<StatusResponse>(url, request, token);

                return result;
            }
            catch (Exception ex)
            {
                return new StatusResponse { Code = 500, Message = ex.Message };
            }
        }
        internal async Task<StatusResponse> DeleteUser(UsernameAndRequester request, string token)
        {
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "user/delete";

                var result = await GeneralAction.PostAsync<StatusResponse>(url, request, token);

                return result;
            }
            catch (Exception ex)
            {
                return new StatusResponse { Code = 500, Message = ex.Message };
            }
        }
        internal async Task<User> UpdateUser(UpdateUserRequest request, string token)
        {
            try
            {

                // Logic for new user update

                var url = GeneralAction.GetBaseAPIUrl() + "user/update";

                var result = await GeneralAction.PostAsync<User>(url, request, token);

                return result;
            }
            catch (Exception ex)
            {
                return new User { Code = 500, Message = ex.Message };
            }
        }

        internal async Task<User> ViewUserDetail(string username, string token)
        {   
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "user/" + username;
                var result = await GeneralAction.GetAsync<User>(url, token);

                return result;
            }
            catch (Exception ex)
            {
                return new User { Code = 500, Message = ex.Message };
            }
        }
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }
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

    public class UsernameAndRequester
    {
        public string Username { get; set; }
        public string Requester { get; set; }
    }

    public class UserInfoRequestToAPI
    {
        public string SpecialKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ShopName { get; set; }
        public string Role { get; set; }
        public string ShopDesc { get; set; }
        public string ShopLocation { get; set; }
        public string OpenCloseTime { get; set; }
        public HttpPostedFileBase ImgUpload { get; set; }
        public string ShopImg { get; set; }
        public string TelegramId { get; set; }
        public string Social { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Performer { get; set; }
        public string Username { get; set; }
        public string ShopName { get; set; }
        public string ShopDesc { get; set; }
        public string ShopLocation { get; set; }
        public string OpenCloseTime { get; set; }
        public string ImgName { get; set; }
        public string ImgData { get; set; }
        public string TelegramId { get; set; }
        public string Social { get; set; }
    }
}