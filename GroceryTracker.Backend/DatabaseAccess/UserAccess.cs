using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IUserAccess : IAccessBase<DbUser>
   {
      /// <summary>
      /// Returns a whole user record that matches the provided username. Else returns null.
      /// </summary>
      Task<DbUser> GetUserByUsername(string username);

      /// <summary>
      /// Checks whether a given email address already exists in the database
      /// </summary>
      Task<bool> IsEmailUnique(string email);

      /// <summary>
      /// Checks whether a given username already exists in the database
      /// </summary>
      Task<bool> IsUsernameUnique(string username);
   }

   public class UserAccess : AccessBase<DbUser>, IUserAccess
   {
      public UserAccess(string host, int port, string databaseName, string username, string password)
      : base(host, port, databaseName, username, password, new DbEntityTypeInfo<DbUser>())
      {
      }

      public async Task<bool> IsEmailUnique(string email)
      {
         var query = new Query(this.EntityTypeInfo.Name).Where(this.EntityTypeInfo.Props[nameof(DbUser.Email)], email);
         var compiledQuery = this.SqlServerCompiler.Compile(query);

         return (await this.GetSingleAsync(compiledQuery.Sql)) != null;
      }

      public async Task<bool> IsUsernameUnique(string username)
      {
         var query = new Query(this.EntityTypeInfo.Name).Where(this.EntityTypeInfo.Props[nameof(DbUser.Username)], username);
         var compiledQuery = this.SqlServerCompiler.Compile(query);

         return (await this.GetSingleAsync(compiledQuery.Sql)) != null;
      }

      public async Task<DbUser> GetUserByUsername(string username)
      {
         var query = new Query(this.EntityTypeInfo.Name).Where(this.EntityTypeInfo.Props[nameof(DbUser.Username)], username);
         var compiledQuery = this.SqlServerCompiler.Compile(query);

         return await this.GetSingleAsync(compiledQuery.Sql);
      }
   }
}