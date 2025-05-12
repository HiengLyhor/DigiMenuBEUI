using MyDigiMenu.Models;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyDigiMenu.Controllers
{
    public class UserController : Controller
    {

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Super"].ToString().Equals("USER")) return RedirectToAction("All", "Recipe");
            return View(new UserInfoRequestToAPI());
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserInfoRequestToAPI request)
        {
            if (ModelState.IsValid)
            {

                if (Session["Super"].ToString().Equals("USER"))
                {
                    TempData["ErrorMessage"] = "You don't have permission to perform this action!";
                    return RedirectToAction("All", "Recipe");
                }

                try
                {

                    // Perform action create new user
                    string adminString = GeneralAction.GetAdminRight();
                    if (request.ImgUpload != null && request.ImgUpload.ContentType == "image/png")
                    {
                        var compressedBase64 = GeneralAction.CompressAndConvertToBase64(request.ImgUpload);
                        request.ShopImg = compressedBase64;
                    }

                    request.Password = Encryption.Encrypt(request.Password);
                    request.SpecialKey = adminString;

                    // Post API
                    StatusResponse result = await new User().CreateUser(request, Session["Token"]?.ToString());

                    if (result.Code == (int)HttpStatusCode.OK)
                    {
                        TempData["SuccessMessage"] = "User created successfully!";

                    } else
                    {
                        TempData["ErrorMessage"] = result.Message;
                    }
                }
                catch(Exception ex)
                {
                    GeneralAction.SendMessageAsync("POST UserController.Create " + ex.Message).Wait();
                    TempData["ErrorMessage"] = ex.Message;
                }

            }

            return RedirectToAction("All");
        }

        [HttpGet]
        public ActionResult ViewUser(string username)
        {

            bool selfView = Session["Super"].ToString().Equals("USER") && Session["User"].ToString().Equals(username);
            bool adminView = Session["Super"].ToString().Equals("ADMIN");

            if (selfView || adminView)
            {
                // Get data to show in interface
                return View();
            }
            return RedirectToAction("/");

        }

        [HttpGet]
        public ActionResult Edit(string username)
        {

            bool selfView = Session["Super"].ToString().Equals("USER") && Session["User"].ToString().Equals(username);
            bool adminView = Session["Super"].ToString().Equals("ADMIN");

            if (selfView || adminView)
            {
                // Get data to show in interface
                return View();
            }
            return RedirectToAction("/");

        }

        [HttpPost]
        public ActionResult Edit(UserInfoRequestToAPI user)
        {

            bool selfView = Session["Super"].ToString().Equals("USER") && Session["User"].ToString().Equals(user.Username);
            bool adminView = Session["Super"].ToString().Equals("ADMIN");

            if (selfView || adminView)
            {
                // Post method to edit user info
                return View();
            }
            return RedirectToAction("/");

        }

        [HttpPost]
        public ActionResult Delete(string username)
        {
            bool admin = Session["Super"].ToString().Equals("ADMIN");

            if (admin)
            {
                // Perform call API to delete record
                return View();
            }
            return RedirectToAction("/");

        }

        [HttpGet]
        public ActionResult All()
        {
            if (Session["Super"].ToString().Equals("USER")) return RedirectToAction("All", "Recipe");
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> All(int draw, int start, int length, string searchValue)
        {
            try
            {
                // Authorization check
                if (Session["Super"]?.ToString() == "USER")
                    return Json(new
                    {
                        draw = draw,
                        error = "Unauthorized access"
                    }, JsonRequestBehavior.AllowGet);

                // Get data from service
                User user = new User();
                var apiResponse = await user.GetAllUsers(
                    searchValue,
                    Session["Token"]?.ToString(),
                    Session["User"]?.ToString()
                );

                if (apiResponse.Code != 200)
                {
                    return Json(new
                    {
                        draw = draw,
                        error = apiResponse.Message
                    }, JsonRequestBehavior.AllowGet);
                }

                DataSet ds = user.ConvertAllUserResponseToDataSet(apiResponse);

                // Prepare response
                var response = new
                {
                    draw = draw,
                    recordsTotal = ds.Tables[1].Rows.Count > 0 ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0,
                    recordsFiltered = ds.Tables[1].Rows.Count > 0 ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0,
                    data = new GeneralAction().DataTableToList(ds.Tables[0]) // Convert DataTable to list of dictionaries
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    draw = Request.Form["draw"],
                    error = $"An error occurred: {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}