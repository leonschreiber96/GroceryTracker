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
      public async Task<IActionResult> GetAll()
      {
         var categories = await this.categoryAccess.GetAllAsync();

         return Ok(categories);
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] CategoryDto categoryDto)
      {
         var targetCategory = await this.categoryAccess.GetSingleAsync(categoryDto.Id);

         if (targetCategory == null) return NotFound("Category does not exist in database.");

         if (targetCategory.ParentId != categoryDto.Parent)
         {
            // Validate new parent category
         }

         var category = new DbCategory
         {
            Id = -1, // Id Id is ignored and set to 0, the entity with id 0 will be updated instead of insert operation
            Name = categoryDto.Name,
            ParentId = categoryDto.Parent
         };

         try
         {
            await this.categoryAccess.Update(category);
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
         // Validate new parent category

         var category = new DbCategory
         {
            Name = categoryDto.Name,
            ParentId = categoryDto.Parent
         };

         try
         {
            await this.categoryAccess.Insert(category);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Category info changed successfully!");
      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromQuery] int categoryId)
      {
         var targetCategory = await this.categoryAccess.GetSingleAsync(categoryId);

         if (targetCategory == null) return NotFound("Category does not exist in database.");

         try
         {
            await this.categoryAccess.Delete(categoryId);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Category info changed successfully!");
      }
   }
}
