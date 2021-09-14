using System;
using System.Threading.Tasks;
using GroceryTracker.Backend.DatabaseAccess;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GroceryTracker.Backend.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class ArticleController : ControllerBase
   {
      private readonly IArticleAccess articleAccess;

      public ArticleController(IArticleAccess articleAccess)
      {
         this.articleAccess = articleAccess;
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] ArticleDto articleDto)
      {
         var targetArticle = await this.articleAccess.GetSingleAsync(articleDto.ArticleId);

         if (targetArticle == null) return NotFound("Article does not exist in database.");

         if (targetArticle.CategoryId != articleDto.CategoryId)
         {
            // TODO: Validate new category
         }

         if (targetArticle.BrandId != articleDto.BrandId)
         {
            // TODO: Validate new brand
         }

         var article = new DbArticle
         {
            Id = articleDto.ArticleId,
            Name = articleDto.Name,
            BrandId = articleDto.BrandId,
            CategoryId = articleDto.CategoryId,
            Details = articleDto.Details,
            OwnerId = articleDto.OwnerId,
            Pfand = articleDto.Pfand,
            Tags = articleDto.Tags
         };

         try
         {
            await this.articleAccess.UpdateAsync(article);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Article info changed successfully!");
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromForm] ArticleDto articleDto)
      {
         // TODO: Validate new category

         // TODO: Validate new brand

         var article = new DbArticle
         {
            Name = articleDto.Name,
            BrandId = articleDto.BrandId,
            CategoryId = articleDto.CategoryId,
            Details = articleDto.Details,
            OwnerId = articleDto.OwnerId,
            Pfand = articleDto.Pfand,
            Tags = articleDto.Tags
         };

         try
         {
            var newId = await this.articleAccess.InsertAsync(article);

            return Created($"/article/{newId}", new { message = "Article created successfully!", newId });
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromQuery] int articleId)
      {
         var targetArticle = await this.articleAccess.GetSingleAsync(articleId);

         if (targetArticle == null) return NotFound("Article does not exist in database.");

         try
         {
            await this.articleAccess.DeleteAsync(articleId);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Article deleted successfully!");
      }
   }
}
