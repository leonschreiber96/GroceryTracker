using Microsoft.AspNetCore.Identity;

namespace GroceryTracker.Backend.Auth
{
   public class AuthenticatedUser : IdentityUser
   {
      public string Username { get; set; }

      public string FirstName { get; set; }

      public string LastName { get; set; }

      public string AuthToken { get; set; }
   }
}