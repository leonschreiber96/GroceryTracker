using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryTracker.Backend.DatabaseAccess;
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

      public ShoppingTripController(IShoppingTripAccess shoppingTripAccess)
      {
         this.shoppingTripAccess = shoppingTripAccess;
      }

      [HttpGet]
      public async Task<IActionResult> Get([FromQuery] int tripId)
      {
         var result = await this.shoppingTripAccess.GetSingleAsync(tripId);

         if (result == null) return NotFound("Shopping Trip does not exist in the database:");

         return Ok(result);
      }

      [HttpGet]
      [Route("all")]
      public IActionResult GetAll([FromQuery] int limit = 30)
      {
         throw new NotImplementedException();
      }

      [HttpPut]
      public IActionResult Put([FromBody] ShoppingTripMetaDto articleDto)
      {
         throw new NotImplementedException();
      }

      [HttpPost]
      public IActionResult Post([FromBody] ShoppingTripMetaDto articleDto)
      {
         throw new NotImplementedException();
      }

      [HttpDelete]
      public IActionResult Delete([FromQuery] int id)
      {
         throw new NotImplementedException();
      }
   }
}
