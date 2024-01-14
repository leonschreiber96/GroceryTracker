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
      private readonly ICategoryAccess categoryAccess;
      private readonly IBrandAccess brandAccess;

      public ArticleController(IArticleAccess articleAccess, ICategoryAccess categoryAccess, IBrandAccess brandAccess)
      {
         this.articleAccess = articleAccess;
         this.categoryAccess = categoryAccess;
         this.brandAccess = brandAccess;
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] ArticleDto articleDto)
      {
         var targetArticle = await this.articleAccess.GetSingleAsync(articleDto.Id);

         if (targetArticle == null) return NotFound("Article does not exist in database.");

         if (targetArticle.CategoryId != articleDto.CategoryId)
         {
            if (!await this.categoryAccess.ExistsAsync(articleDto.CategoryId)) return NotFound("Provided category does not exist in the database.");
         }

         if (targetArticle.BrandId != articleDto.BrandId && articleDto.BrandId != null)
         {
            if (!await this.brandAccess.ExistsAsync((int)articleDto.BrandId)) return NotFound("Provided brand does not exist in the database.");
         }

         var article = new DbArticle
         {
            Id = articleDto.Id,
            Name = articleDto.Name,
            BrandId = articleDto.BrandId,
            CategoryId = articleDto.CategoryId,
            Details = articleDto.Details,
            Pfand = articleDto.Pfand,
            Tags = articleDto.Tags,
            OwnerId = 1
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
         if (!await this.categoryAccess.ExistsAsync(articleDto.CategoryId)) return NotFound("Provided category does not exist in database.");
         if (!await this.brandAccess.ExistsAsync((int)articleDto.BrandId)) return NotFound("Provided brand does not exist in database.");

         var article = new DbArticle
         {
            Name = articleDto.Name,
            BrandId = articleDto.BrandId,
            CategoryId = articleDto.CategoryId,
            Details = articleDto.Details,
            Pfand = articleDto.Pfand,
            Tags = articleDto.Tags,
            OwnerId = 1
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
