using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GroceryTracker.Backend.DatabaseAccess;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using Microsoft.AspNetCore.Mvc;

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
      public async Task<IActionResult> Get([FromQuery] int userId)
      {
         var targetUser = await this.userAccess.GetSingleAsync(userId);

         if (targetUser == null) return NotFound("User does not exist in the database:");

         return Ok(targetUser);
      }

      [HttpPut]
      public async Task<IActionResult> Put([FromForm] UserDto userDto)
      {
         var targetUser = await this.userAccess.GetSingleAsync(userDto.UserId);

         if (targetUser == null) return NotFound("User does not exist in the database:");

         if (userDto.Email != targetUser.Email)
         {
            // Check provided email
            if (string.IsNullOrEmpty(userDto.Email)) return BadRequest("Email field can't be empty.");
            if (!this.IsMailAddress(userDto.Email)) return BadRequest("Email field contains no valid e-mail address.");
            if (!await this.userAccess.IsEmailUnique(userDto.Email)) return BadRequest("Email-address is already in use.");
         }

         if (userDto.Username != targetUser.Username)
         {
            // Check provided username
            if (string.IsNullOrWhiteSpace(userDto.Username)) return BadRequest("Username can't be empty");
            if (!await this.userAccess.IsUsernameUnique(userDto.Username)) return BadRequest("Username is already in use.");
         }

         var salt = BCrypt.Net.BCrypt.GenerateSalt();
         var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password, salt);

         var user = new DbAppUser
         {
            Id = userDto.UserId,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Username = userDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = salt
         };

         try
         {
            await this.userAccess.Update(user);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("User info changed successfully!");
      }

      /// <summary>
      /// Creates a new user from registration form input data. Input values are checked for validity.
      /// </summary>
      [HttpPost]
      public async Task<IActionResult> Post([FromForm] UserDto userDto)
      {
         // Check provided email
         if (string.IsNullOrEmpty(userDto.Email)) return BadRequest("Email field can't be empty.");
         if (!this.IsMailAddress(userDto.Email)) return BadRequest("Email field contains no valid e-mail address.");
         if (!await this.userAccess.IsEmailUnique(userDto.Email)) return BadRequest("Email-address is already in use.");

         // Check provided username
         if (string.IsNullOrWhiteSpace(userDto.Username)) return BadRequest("Username can't be empty");
         if (!await this.userAccess.IsUsernameUnique(userDto.Username)) return BadRequest("Username is already in use.");

         var salt = BCrypt.Net.BCrypt.GenerateSalt();
         var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password, salt);

         var newUser = new DbAppUser
         {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Username = userDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = salt
         };

         try
         {
            await this.userAccess.Insert(newUser);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("User created successfully!");
      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromQuery] int userId)
      {
         var targetUser = await this.userAccess.GetSingleAsync(userId);

         if (targetUser == null) return NotFound("User does not exist in the database:");

         try
         {
            await this.userAccess.Delete(userId);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }

         return Ok("User deleted successfully!");
      }

      private bool IsMailAddress(string input)
      {
         var mailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
         return mailRegex.IsMatch(input);
      }
   }
}
