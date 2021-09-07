using System.Security.Cryptography;
using System.Threading.Tasks;
using GroceryTracker.Backend.DatabaseAccess;
using GroceryTracker.Backend.ExtensionMethods;

namespace GroceryTracker.Backend.Auth
{
   public interface IAuthenticationManager
   {
      /// <summary>
      /// Provided a username and password: checks if they match
      /// </summary>
      /// <returns>Returns true if authentication was successfull, else false</returns>
      Task<bool> CheckAuthentication(string username, string password);
   }

   public class AuthenticationManager : IAuthenticationManager
   {
      private readonly IUserAccess userAccess;

      public AuthenticationManager(IUserAccess userAccess)
      {
         this.userAccess = userAccess;
      }

      public async Task<bool> CheckAuthentication(string username, string password)
      {
         var matchingUser = await this.userAccess.GetUserByUsername(username);

         var trueHash = matchingUser.PasswordHash;
         var salt = matchingUser.PasswordSalt;

         var hash = password.Sha256Salted(salt);

         if (hash.Length != trueHash.Length) return false;

         for (int i = 0; i < hash.Length; i++)
         {
            if (hash[i] != trueHash[i]) return false;
         }

         return true;
      }
   }
}