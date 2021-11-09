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
         var frequent = await purchaseAccess.GetFrequentAsync(marketId, limit);
         return Ok(frequent);
      }

      [HttpGet]
      [Route("recent")]
      public async Task<IActionResult> GetRecent([FromQuery] int marketId, [FromQuery] int limit = 30)
      {
         var recent = await purchaseAccess.GetRecentAsync(marketId, limit);
         return Ok(recent);
      }

      [HttpPut]
      public async Task<IActionResult> EditPurchase([FromBody] PurchaseDto purchaseDto)
      {
         var tcs = new TaskCompletionSource<IActionResult>();
         tcs.SetException(new NotImplementedException());
         return await tcs.Task;
      }

      [HttpPost]
      public async Task<IActionResult> AddPurchaseToTrip([FromQuery] int tripId, [FromBody] PurchaseDto purchaseDto)
      {
         var tcs = new TaskCompletionSource<IActionResult>();
         tcs.SetException(new NotImplementedException());
         return await tcs.Task;
      }

      [HttpDelete]
      public async Task<IActionResult> RemovePurchaseFromTrip([FromQuery] int tripId, [FromQuery] int purchaseId)
      {
         var tcs = new TaskCompletionSource<IActionResult>();
         tcs.SetException(new NotImplementedException());
         return await tcs.Task;
      }
   }
}
