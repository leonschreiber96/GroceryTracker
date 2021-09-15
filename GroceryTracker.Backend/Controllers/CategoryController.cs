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
   public class CategoryController : ControllerBase
   {
      private readonly ICategoryAccess categoryAccess;

      public CategoryController(ICategoryAccess categoryAccess)
      {
         this.categoryAccess = categoryAccess;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll([FromQuery] int limit = 30)
      {
         var categories = await this.categoryAccess.GetAllAsync(limit);

         return Ok(categories);
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] CategoryDto categoryDto)
      {
         var targetCategory = await this.categoryAccess.GetSingleAsync(categoryDto.CategoryId);

         if (targetCategory == null) return NotFound("Category does not exist in database.");

         if (targetCategory.ParentId != categoryDto.Parent)
         {
            var parent = this.categoryAccess.GetSingleAsync((int)categoryDto.Parent);
            if (parent == null) return NotFound("Parent category does not exist in database.");
         }

         var category = new DbCategory
         {
            Id = categoryDto.CategoryId,
            Name = categoryDto.Name ?? targetCategory.Name,
            ParentId = categoryDto.Parent ?? targetCategory.ParentId,
            OwnerId = 1
         };

         try
         {
            await this.categoryAccess.UpdateAsync(category);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Category info changed successfully!");
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromForm] CategoryDto categoryDto)
      {
         if (categoryDto.Parent != null)
         {
            var parent = this.categoryAccess.GetSingleAsync((int)categoryDto.Parent);
            if (parent == null) return NotFound("Parent category does not exist in database.");
         }

         var category = new DbCategory
         {
            Name = categoryDto.Name,
            ParentId = categoryDto.Parent,
            OwnerId = 1
         };

         try
         {
            var newId = await this.categoryAccess.InsertAsync(category);

            return Created($"/category/{newId}", new { message = "Category created successfully!", newId });
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }
      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromQuery] int categoryId)
      {
         var targetCategory = await this.categoryAccess.GetSingleAsync(categoryId);

         if (targetCategory == null) return NotFound("Category does not exist in database.");

         try
         {
            await this.categoryAccess.DeleteAsync(categoryId);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Category deleted successfully!");
      }
   }
}
