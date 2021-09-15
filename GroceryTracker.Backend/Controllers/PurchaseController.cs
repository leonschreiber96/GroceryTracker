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
   public class PurchaseController : ControllerBase
   {
      private readonly IPurchaseAccess purchaseAccess;
      private readonly IShoppingTripAccess tripAccess;

      public PurchaseController(IPurchaseAccess purchaseAccess, IShoppingTripAccess tripAccess)
      {
         this.purchaseAccess = purchaseAccess;
         this.tripAccess = tripAccess;
      }

      [HttpGet]
      [Route("frequent")]
      public async Task<IActionResult> GetFrequent([FromQuery] int marketId, [FromQuery] int limit = 30)
      {
         throw new NotImplementedException();
      }

      [HttpGet]
      [Route("recent")]
      public async Task<IActionResult> GetRecent([FromQuery] int marketId, [FromQuery] int limit = 30)
      {
         throw new NotImplementedException();
      }

      [HttpPut]
      public async Task<IActionResult> EditPurchase([FromBody] PurchaseDto purchaseDto)
      {
         throw new NotImplementedException();
      }

      [HttpPost]
      public async Task<IActionResult> AddPurchaseToTrip([FromQuery] int tripId, [FromBody] PurchaseDto purchaseDto)
      {
         throw new NotImplementedException();
      }

      [HttpDelete]
      public async Task<IActionResult> RemovePurchaseFromTrip([FromQuery] int tripId, [FromQuery] int purchaseId)
      {
         throw new NotImplementedException();
      }
   }
}
