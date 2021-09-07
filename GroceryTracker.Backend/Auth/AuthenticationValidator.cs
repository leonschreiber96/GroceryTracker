namespace GroceryTracker.Backend.Auth
{
   public interface IAuthenticationValidator
   {
      /// <summary>
      /// Provided a username and password: checks if they match
      /// </summary>
      /// <returns>Returns true if authentication was successfull, else false</returns>
      bool CheckAuthentication(string username, string password);
   }
}