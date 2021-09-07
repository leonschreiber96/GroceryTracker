using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IUserAccess
   {
      Task<string> GetPasswordHash(int userId);
   }

   public class UserAccess : AccessBase<DbUser>, IUserAccess
   {
      public UserAccess(string host, int port, string databaseName, string username, string password) : base(host, port, databaseName, username, password)
      {
      }

      public async Task<string> GetPasswordHash(int userId) => (await base.GetSingleAsync(userId)).PasswordHash;
   }
}