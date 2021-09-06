using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroceryTracker.Backend.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class PurchaseController : ControllerBase
   {
      public PurchaseController()
      {
      }

      [HttpGet]
      [Route("frequent")]
      public IActionResult GetFrequent([FromQuery] int marketId, [FromQuery] int limit = 30)
      {
         throw new NotImplementedException();
      }

      [HttpGet]
      [Route("recent")]
      public IActionResult GetRecent([FromQuery] int marketId, [FromQuery] int limit = 30)
      {
         throw new NotImplementedException();
      }

      [HttpPut]
      public IActionResult EditPurchase([FromBody] PurchaseDto articleDto)
      {
         throw new NotImplementedException();
      }

      [HttpPost]
      public IActionResult AddPurchaseToTrip([FromQuery] int tripId, [FromBody] PurchaseDto purchase)
      {
         throw new NotImplementedException();
      }

      [HttpDelete]
      public IActionResult RemovePurchaseFromTrip([FromQuery] int tripId, [FromQuery] int purchaseId)
      {
         throw new NotImplementedException();
      }
   }
}
