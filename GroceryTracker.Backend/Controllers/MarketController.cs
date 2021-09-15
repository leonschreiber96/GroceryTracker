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
   public class MarketController : ControllerBase
   {
      private readonly IMarketAccess marketAccess;

      public MarketController(IMarketAccess marketAccess)
      {
         this.marketAccess = marketAccess;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll([FromQuery] int limit = 30)
      {
         var categories = await this.marketAccess.GetAllAsync(limit);

         return Ok(categories);
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] MarketDto marketDto)
      {
         var targetMarket = await this.marketAccess.GetSingleAsync(marketDto.MarketId);

         if (targetMarket == null) return NotFound("Market does not exist in database.");

         var market = new DbMarket
         {
            Id = marketDto.MarketId,
            Name = marketDto.Name,
            OwnerId = 1
         };

         try
         {
            await this.marketAccess.UpdateAsync(market);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Market info changed successfully!");
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromForm] MarketDto marketDto)
      {
         var market = new DbMarket
         {
            Name = marketDto.Name,
            OwnerId = 1
         };

         try
         {
            var newId = await this.marketAccess.InsertAsync(market);

            return Created($"/market/{newId}", new { message = "Market created successfully!", newId });
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }
      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromQuery] int marketId)
      {
         var targetMarket = await this.marketAccess.GetSingleAsync(marketId);

         if (targetMarket == null) return NotFound("Market does not exist in database.");

         try
         {
            await this.marketAccess.DeleteAsync(marketId);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Market deleted successfully!");
      }
   }
}
