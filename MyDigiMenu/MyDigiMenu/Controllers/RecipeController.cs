using MyDigiMenu.Models;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using System.Net;

namespace MyDigiMenu.Controllers
{
    public class RecipeController : Controller
    {
        [HttpGet]
        public ActionResult All()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> All(int draw, int start, int length, string searchValue)
        {
            try
            {
                
                // Get data from service
                Recipe recipe = new Recipe();
                var apiResponse = await recipe.GetRecipeList(
                    Session["ShopKey"]?.ToString(),
                    searchValue,
                    Session["Token"]?.ToString()
                );

                if (apiResponse.Code != 200)
                {
                    return Json(new
                    {
                        draw = draw,
                        error = apiResponse.Message
                    }, JsonRequestBehavior.AllowGet);
                }

                DataSet ds = recipe.ConvertAllRecipeResponseToDataSet(apiResponse);

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

        [HttpPost]
        public async Task<JsonResult> Active(int id)
        {
            ShopKeyAndRequeseter shopKeyAndRequeseter = new ShopKeyAndRequeseter
            {
                RecipeId = id,
                ShopKey = Session["ShopKey"].ToString(),
                Requester = Session["User"]?.ToString()
            };

            StatusResponse result = await new Recipe().EnableRecipe(shopKeyAndRequeseter, Session["Token"]?.ToString());

            if (result.Code != (int)HttpStatusCode.OK)
            {
                string stuats = "inactive";
                return Json(new { success = false, message = result.Message, status = stuats });
            }

            string status = "active";
            return Json(new { success = true, status = status });
        }

        [HttpPost]
        public async Task<JsonResult> Inactive(int id)
        {
            ShopKeyAndRequeseter shopKeyAndRequeseter = new ShopKeyAndRequeseter
            {
                RecipeId = id,
                ShopKey = Session["ShopKey"].ToString(),
                Requester = Session["User"]?.ToString()
            };

            StatusResponse result = await new Recipe().DisableRecipe(shopKeyAndRequeseter, Session["Token"]?.ToString());

            if (result.Code != (int)HttpStatusCode.OK)
            {
                string stuats = "active";
                return Json(new { success = false, message = result.Message, status = stuats });
            }

            string status = "inactive";
            return Json(new { success = true, status = status });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            ShopKeyAndRequeseter shopKeyAndRequeseter = new ShopKeyAndRequeseter
            {
                RecipeId = id,
                ShopKey = Session["ShopKey"].ToString(),
                Requester = Session["User"]?.ToString()
            };
            StatusResponse result = await new Recipe().DeleteRecipe(shopKeyAndRequeseter, Session["Token"]?.ToString());

            if (result.Code == (int)HttpStatusCode.OK)
            {
                TempData["SuccessMessage"] = "User created successfully!";

            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("All");
            
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateRecipeRequest());
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecipe(CreateRecipeRequest request)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    // Perform action create new recipe
                    if (request.ImgUpload != null && request.ImgUpload.ContentType == "image/png")
                    {
                        var compressedBase64 = GeneralAction.CompressAndConvertToBase64(request.ImgUpload);
                        request.ImgData = compressedBase64;
                        request.ImgUpload = null;
                    }

                    request.Username = Session["User"]?.ToString();
                    // Post API
                    StatusResponse result = await new Recipe().CreateRecipe(request, Session["Token"]?.ToString());

                    if (result.Code == (int)HttpStatusCode.OK)
                    {
                        TempData["SuccessMessage"] = "Recipe created successfully!";

                    }
                    else
                    {
                        TempData["ErrorMessage"] = result.Message;
                    }
                }
                catch (Exception ex)
                {
                    GeneralAction.SendMessageAsync("POST UserController.Create " + ex.Message).Wait();
                    TempData["ErrorMessage"] = ex.Message;
                }

            }

            return RedirectPermanent("All");
        }

        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            SingleRecipeResponse singleRecipe = await new Recipe().GetSingleRecipe(id);
            return View(singleRecipe);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateRecipeRequest request)
        {
            request.Tags = request.Tag;
            request.Username = Session["User"]?.ToString();

            if (request.ImgUpload != null && request.ImgUpload.ContentType == "image/png")
            {
                var compressedBase64 = GeneralAction.CompressAndConvertToBase64(request.ImgUpload);
                request.ImgName = "CHANGED";
                request.ImgData = compressedBase64;
                request.ImgUpload = null; // Set because when post to API error
            }
            
            SingleRecipeResponse result = await new Recipe().UpdateRecipe(request, Session["Token"]?.ToString());

            if (result.Code == (int)HttpStatusCode.OK)
            {
                TempData["SuccessMessage"] = "Recipe updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectPermanent("All");
        }

        [HttpGet]
        public async Task<ActionResult> View(int id)
        {
            SingleRecipeResponse singleRecipe = await new Recipe().GetSingleRecipe(id);
            return View(singleRecipe);
        }

    }
}