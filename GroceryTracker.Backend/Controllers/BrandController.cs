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
   public class BrandController : ControllerBase
   {
      private readonly IBrandAccess brandAccess;

      public BrandController(IBrandAccess brandAccess)
      {
         this.brandAccess = brandAccess;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll([FromQuery] int limit = 30)
      {
         var categories = await this.brandAccess.GetAllAsync(limit);

         return Ok(categories);
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] BrandDto brandDto)
      {
         var targetBrand = await this.brandAccess.GetSingleAsync(brandDto.Id);

         if (targetBrand == null) return NotFound("Brand does not exist in database.");

         var brand = new DbBrand
         {
            Id = brandDto.Id,
            Name = brandDto.Name,
            OwnerId = 1
         };

         try
         {
            await this.brandAccess.UpdateAsync(brand);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Brand info changed successfully!");
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromForm] BrandDto brandDto)
      {
         var brand = new DbBrand
         {
            Name = brandDto.Name,
            OwnerId = 1
         };

         try
         {
            var newId = await this.brandAccess.InsertAsync(brand);

            return Created($"/brand/{newId}", new { message = "Brand info changed successfully!", newId });
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }
      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromQuery] int brandId)
      {
         var targetBrand = await this.brandAccess.GetSingleAsync(brandId);

         if (targetBrand == null) return NotFound("Brand does not exist in database.");

         try
         {
            await this.brandAccess.DeleteAsync(brandId);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Brand deleted successfully!");
      }
   }
}
