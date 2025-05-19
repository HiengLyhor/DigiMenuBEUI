using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDigiMenu.Models
{
    public class Recipe
    {

        public SingleRecipeResponse CreateRecipe(CreateRecipeRequest recipeRequest)
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

        public SingleRecipeResponse UpdateRecipe(UpdateRecipeRequest recipeRequest) 
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

        public StatusResponse EnableRecipe (ShopKeyAndRequeseter request)
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
        public StatusResponse DisableRecipe(ShopKeyAndRequeseter request)
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

        public StatusResponse DeleteRecipe(ShopKeyAndRequeseter request)
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

        public async Task<RecipeListResponse> GetRecipeList(string shopKey, string searchVal, string token, string requeseter)
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
        public int recipeId { get; set; }
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
        public List<string> Tags { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
    }

    public class RecipeListResponse : StatusResponse
    {
        public string ShopKey { get; set; }
        public List<SingleRecipeResponse> ListRecipe { get; set; }
    }

}