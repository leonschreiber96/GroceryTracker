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
   public class MarketController : ControllerBase
   {
      public MarketController()
      {
      }

      [HttpGet]
      public IActionResult GetAll()
      {
         throw new NotImplementedException();
      }

      [HttpPut]
      public IActionResult Put([FromBody] MarketDto articleDto)
      {
         throw new NotImplementedException();
      }

      [HttpPost]
      public IActionResult Post([FromBody] MarketDto articleDto)
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
