using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GroceryTracker.Backend.DatabaseAccess;
using GroceryTracker.Backend.ExtensionMethods;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroceryTracker.Backend.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class UserController : ControllerBase
   {
      private readonly IUserAccess userAccess;

      public UserController(IUserAccess userAccess)
      {
         this.userAccess = userAccess;
      }

      [HttpGet]
      public IActionResult Get([FromQuery] int id)
      {
         throw new NotImplementedException();
      }

      [HttpPut]
      public IActionResult Put([FromBody] UserDto userDto)
      {
         throw new NotImplementedException();
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody] UserDto userDto)
      {
         // Check provided email
         if (string.IsNullOrEmpty(userDto.Email)) return BadRequest("Email field can't be empty.");
         if (!this.IsMailAddress(userDto.Email)) return BadRequest("Email field contains no valid e-mail address.");
         if (!await this.userAccess.IsEmailUnique(userDto.Email)) return BadRequest("Email-address is already in use.");

         // Check provided username
         if (string.IsNullOrWhiteSpace(userDto.Username)) return BadRequest("Username can't be empty");
         if (!await this.userAccess.IsUsernameUnique(userDto.Username)) return BadRequest("Username is already in use.");

         var salt = Guid.NewGuid().ToByteArray();
         var passwordHash = userDto.Password.Sha256Salted(salt);

         var newUser = new DbUser
         {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Username = userDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = salt
         };

         await this.userAccess.Upsert(newUser);

         return Ok();
      }

      [HttpDelete]
      public IActionResult Delete([FromQuery] int id)
      {
         throw new NotImplementedException();
      }

      private bool IsMailAddress(string input)
      {
         var mailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
         return mailRegex.IsMatch(input);
      }
   }
}
