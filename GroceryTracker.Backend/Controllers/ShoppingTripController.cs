using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryTracker.Backend.DatabaseAccess;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroceryTracker.Backend.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class ShoppingTripController : ControllerBase
   {
      private readonly IShoppingTripAccess shoppingTripAccess;
      private readonly IMarketAccess marketAccess;

      public ShoppingTripController(IShoppingTripAccess shoppingTripAccess, IMarketAccess marketAccess)
      {
         this.shoppingTripAccess = shoppingTripAccess;
         this.marketAccess = marketAccess;
      }

      [HttpGet]
      public async Task<IActionResult> Get([FromQuery] int tripId)
      {
         var result = await this.shoppingTripAccess.GetSingleDetailed(tripId);

         if (result == null) return NotFound("Shopping Trip does not exist in the database:");

         return Ok(result);
      }

      [HttpGet]
      [Route("all")]
      public async Task<IActionResult> GetAll([FromQuery] int limit = 30)
      {
         var categories = await this.shoppingTripAccess.GetAllAsync(limit);

         return Ok(categories);
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] ShoppingTripMetaDto tripDto)
      {
         var targetTrip = await this.shoppingTripAccess.GetSingleAsync(tripDto.TripId);

         if (targetTrip == null) return NotFound("Shopping Trip does not exist in database.");

         if (targetTrip.MarketId != tripDto.MarketId)
         {
            if (!await this.marketAccess.ExistsAsync((int)tripDto.MarketId)) return NotFound("Provided market does not exist in database.");
         }

         var trip = new DbShoppingTrip
         {
            Id = tripDto.TripId,
            MarketId = tripDto.MarketId,
            Timestamp = tripDto.Timestamp,
            OwnerId = 1
         };

         try
         {
            await this.shoppingTripAccess.UpdateAsync(trip);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Shopping trip info changed successfully!");
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromForm] ShoppingTripMetaDto tripDto)
      {
         if (!await this.marketAccess.ExistsAsync(tripDto.MarketId)) return NotFound("Provided market does not exist in database.");

         var trip = new DbShoppingTrip
         {
            MarketId = tripDto.MarketId,
            Timestamp = tripDto.Timestamp,
            OwnerId = 1
         };

         try
         {
            var newId = await this.shoppingTripAccess.InsertAsync(trip);

            return Created($"/shoppingtrip/{newId}", new { message = "Shopping Trip created successfully!", newId });
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }
      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromQuery] int tripId)
      {
         var targetArticle = await this.shoppingTripAccess.GetSingleAsync(tripId);

         if (targetArticle == null) return NotFound("Shopping trip does not exist in database.");

         try
         {
            await this.shoppingTripAccess.DeleteAsync(tripId);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("Shopping trip deleted successfully!");
      }
   }
}
