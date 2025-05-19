using MyDigiMenu.Models;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;

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

                //DataSet ds = user.ConvertAllUserResponseToDataSet(apiResponse);
                DataSet ds = null;

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