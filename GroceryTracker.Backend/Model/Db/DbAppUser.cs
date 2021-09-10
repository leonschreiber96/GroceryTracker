namespace GroceryTracker.Backend.Model.Db
{
   public record DbAppUser
   {
      public int Id { get; set; }

      public string Username { get; set; }

      public string FirstName { get; set; }

      public string LastName { get; set; }

      public string Email { get; set; }

      public string PasswordHash { get; set; }

      public string PasswordSalt { get; set; }
   }
}