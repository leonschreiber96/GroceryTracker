using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IUserAccess : IAccessBase<DbAppUser>
   {
      /// <summary>
      /// Returns a whole user record that matches the provided username. Else returns null.
      /// </summary>
      Task<DbAppUser> GetUserByUsername(string username);

      /// <summary>
      /// Checks whether a given email address already exists in the database
      /// </summary>
      Task<bool> IsEmailUnique(string email);

      /// <summary>
      /// Checks whether a given username already exists in the database.
      /// </summary>
      Task<bool> IsUsernameUnique(string username);
   }

   public class UserAccess : AccessBase<DbAppUser>, IUserAccess
   {
      public UserAccess(DatabaseConfiguration configuration) : base(configuration, new DbEntityTypeInfo<DbAppUser>())
      {
      }

      public async Task<bool> IsEmailUnique(string email)
      {
         var query = new Query(this.EntityTypeInfo.Name).Where(this.EntityTypeInfo.Props[nameof(DbAppUser.Email)], email);

         return (await this.GetSingleAsync(query)) == null;
      }

      public async Task<bool> IsUsernameUnique(string username)
      {
         var query = new Query(this.EntityTypeInfo.Name).Where(this.EntityTypeInfo.Props[nameof(DbAppUser.Username)], username);

         return (await this.GetSingleAsync(query)) == null;
      }

      public async Task<DbAppUser> GetUserByUsername(string username)
      {
         var query = new Query(this.EntityTypeInfo.Name).Where(this.EntityTypeInfo.Props[nameof(DbAppUser.Username)], username);

         return await this.GetSingleAsync(query);
      }
   }
}