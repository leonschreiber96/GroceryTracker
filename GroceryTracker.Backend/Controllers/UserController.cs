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
   public class UserController : ControllerBase
   {
      public UserController()
      {
      }

      [HttpGet]
      public IActionResult Get([FromQuery] int id)
      {
         throw new NotImplementedException();
      }

      [HttpPut]
      public IActionResult Put([FromBody] UserDto articleDto)
      {
         throw new NotImplementedException();
      }

      [HttpPost]
      public IActionResult Post([FromBody] UserDto articleDto)
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
