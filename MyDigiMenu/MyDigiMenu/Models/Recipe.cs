using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyDigiMenu.Models
{
    public class Recipe
    {

        public async Task<SingleRecipeResponse> CreateRecipe(CreateRecipeRequest recipeRequest)
        {
            try
            {
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<SingleRecipeResponse> UpdateRecipe(UpdateRecipeRequest recipeRequest) 
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<StatusResponse> EnableRecipe (ShopKeyAndRequeseter request, string token)
        {
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "recipe/enable";

                var result = await GeneralAction.PostAsync<StatusResponse>(url, request, token);

                return result;
            }
            catch (Exception ex)
            {
                return new StatusResponse { Code = 500, Message = ex.Message };
            }
        }
        public async Task<StatusResponse> DisableRecipe(ShopKeyAndRequeseter request, string token)
        {
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "recipe/disable";

                var result = await GeneralAction.PostAsync<StatusResponse>(url, request, token);

                return result;
            }
            catch (Exception ex)
            {
                return new StatusResponse { Code = 500, Message = ex.Message };
            }
        }

        public async Task<StatusResponse> DeleteRecipe(ShopKeyAndRequeseter request, string token)
        {
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "recipe/delete";

                var result = await GeneralAction.PostAsync<StatusResponse>(url, request, token);

                return result;
            }
            catch (Exception ex)
            {
                return new StatusResponse { Code = 500, Message = ex.Message };
            }
        }

        public async Task<RecipeListResponse> GetRecipeList(string shopKey, string searchVal, string token)
        {
            try
            {
                var url = GeneralAction.GetBaseAPIUrl() + "recipe/list/" + shopKey;
                var result = await GeneralAction.GetAsync<RecipeListResponse>(url, token);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SingleRecipeResponse GetSingleRecipe(int id)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ConvertAllRecipeResponseToDataSet(RecipeListResponse response)
        {
            DataSet ds = new DataSet();

            // Create main data table
            DataTable recipeTable = new DataTable("RecipeInfo");

            // Define columns based on your model
            recipeTable.Columns.Add("Id", typeof(int));
            recipeTable.Columns.Add("Name", typeof(string));
            recipeTable.Columns.Add("Description", typeof(string));
            recipeTable.Columns.Add("PriceUsd", typeof(double));
            recipeTable.Columns.Add("PriceKhr", typeof(double));
            recipeTable.Columns.Add("Discount", typeof(int));
            recipeTable.Columns.Add("ImgName", typeof(string));
            recipeTable.Columns.Add("Tags", typeof(string)); // Consider using a more complex type if needed
            recipeTable.Columns.Add("Category", typeof(string));
            recipeTable.Columns.Add("Active", typeof(bool));

            // Populate data
            foreach (var recipe in response.ListRecipe)
            {
                recipeTable.Rows.Add(
                    recipe.Id,
                    recipe.Name,
                    recipe.Description,
                    recipe.PriceUsd,
                    recipe.PriceKhr,
                    recipe.Discount,
                    recipe.ImgName,
                    string.Join(",", recipe.Tag), // Convert list to a comma-separated string
                    recipe.Category,
                    recipe.Active
                );
            }

            // Add count table
            DataTable countTable = new DataTable("Counts");
            countTable.Columns.Add("TotalCount", typeof(int));
            countTable.Rows.Add(response.ListRecipe.Count);

            ds.Tables.Add(recipeTable);
            ds.Tables.Add(countTable);

            return ds;
        }

    }

    public class CreateRecipeRequest
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgData { get; set; }
        public double PriceUsd { get; set; }
        public double PriceKhr { get; set; }
        public int Discount { get; set; }
        public string Tags { get; set; }
        public string Category { get; set; }

    }
    public class UpdateRecipeRequest : CreateRecipeRequest
    {
        int Id { get; set; }
        public string ImgName { get; set; }
    }
    public class ShopKeyAndRequeseter
    {
        public string Requester { get; set; }
        public string ShopKey { get; set; }
        public int RecipeId { get; set; }
    }

    public class SingleRecipeResponse : StatusResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PriceUsd { get; set; }
        public double PriceKhr { get; set; }
        public int Discount { get; set; }
        public string ImgName { get; set; }
        public List<string> Tag { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
    }

    public class RecipeListResponse : StatusResponse
    {
        public string ShopKey { get; set; }
        public List<SingleRecipeResponse> ListRecipe { get; set; }
    }

}