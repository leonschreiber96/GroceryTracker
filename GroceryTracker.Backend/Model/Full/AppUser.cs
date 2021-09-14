using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Full
{
   public record AppUser
   {
      public AppUser(DbAppUser dbObject)
      {
         this.Id = dbObject.Id;
         this.Username = dbObject.Username;
         this.FirstName = dbObject.FirstName;
         this.LastName = dbObject.LastName;
         this.Email = dbObject.Email;
         this.PasswordHash = dbObject.PasswordHash;
         this.PasswordSalt = dbObject.PasswordSalt;
      }

      public int Id { get; set; }

      public string Username { get; set; }

      public string FirstName { get; set; }

      public string LastName { get; set; }

      public string Email { get; set; }

      public string PasswordHash { get; set; }

      public string PasswordSalt { get; set; }
   }
}