namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IDatabaseConfiguration
   {
      string Hostname { get; set; }
      string DatabaseName { get; set; }
      int Port { get; set; }
      string Username { get; set; }
      string Password { get; set; }
   }

   public class DatabaseConfiguration : IDatabaseConfiguration
   {
      public string Hostname { get; set; }

      public string DatabaseName { get; set; }

      public int Port { get; set; }

      public string Username { get; set; }

      public string Password { get; set; }
   }
}