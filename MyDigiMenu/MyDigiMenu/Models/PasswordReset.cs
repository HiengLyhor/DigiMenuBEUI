using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace MyDigiMenu.Models
{
    public class PasswordReset
    {

        public string Username { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        internal async Task<RequestResetResponse> ResetPassword(PasswordReset passwordReset)
        {
            try
            {

                if (passwordReset.Username == null || passwordReset.NewPassword == null) 
                    return new RequestResetResponse { Code = 500, Message = "Invalid username or password." };

                string encryptedPassword = Encryption.Encrypt(passwordReset.NewPassword);

                RequestResetPassword request = new RequestResetPassword { Username = passwordReset.Username, NewPassword = encryptedPassword };

                var url = GeneralAction.GetBaseAPIUrl() + "password-reset/reset";
                var result = await GeneralAction.PostAsync<RequestResetResponse>(url, request, null);

                return result;

            }
            catch (Exception ex)
            {
                await GeneralAction.SendMessageAsync("Error occurred during ResetPassword: " + ex.Message);
                return new RequestResetResponse { Code = 500, Message = "An error occurred during reset password." };
            }
        }

        internal async Task<RequestResetResponse> SendOTP(PasswordReset passwordReset)
        {
            try
            {

                if (passwordReset.Username == null)
                    return new RequestResetResponse { Code = 500, Message = "Invalid username." };

                RequestResetPassword request = new RequestResetPassword{ Username = passwordReset.Username};

                var url = GeneralAction.GetBaseAPIUrl() + "password-reset/send-otp";
                var result = await GeneralAction.PostAsync<RequestResetResponse>(url, request, null);

                return result;

            }
            catch (Exception ex)
            {
                await GeneralAction.SendMessageAsync("Error occurred during SendOTP: " + ex.Message);
                return new RequestResetResponse { Code = 500, Message = "An error occurred during send OTP."};
            }
        }

        internal async Task<RequestResetResponse> VerifyOTP(PasswordReset passwordReset)
        {
            try
            {

                if (passwordReset.Username == null || passwordReset.OTP == null)
                    return new RequestResetResponse { Code = 500, Message = "Invalid username or OTP." };

                string encryptedOTP = Encryption.Encrypt(passwordReset.OTP);

                RequestResetPassword request = new RequestResetPassword { Username = passwordReset.Username, Otp = encryptedOTP };

                var url = GeneralAction.GetBaseAPIUrl() + "password-reset/verify-otp";
                var result = await GeneralAction.PostAsync<RequestResetResponse>(url, request, null);

                return result;

            }
            catch (Exception ex)
            {
                await GeneralAction.SendMessageAsync("Error occurred during VerifyOtp: " + ex.Message);
                return new RequestResetResponse { Code = 500, Message = "An error occurred during verify OTP." };
            }
        }
    }
    class RequestResetPassword
    {
        public string Username { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }

    class RequestResetResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }
}