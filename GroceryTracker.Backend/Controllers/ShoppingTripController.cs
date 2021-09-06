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
   public class ShoppingTripController : ControllerBase
   {
      public ShoppingTripController()
      {
      }

      [HttpGet]
      public IActionResult Get([FromQuery] int id)
      {
         throw new NotImplementedException();
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
