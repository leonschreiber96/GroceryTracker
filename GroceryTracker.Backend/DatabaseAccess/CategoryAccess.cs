using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface ICategoryAccess : IAccessBase<DbCategory>
   {
      Task<IEnumerable<DbCategory>> GetAllAsync(int? limit = 30);
   }

   public class CategoryAccess : AccessBase<DbCategory>, ICategoryAccess
   {
      public CategoryAccess(IDatabaseConfiguration configuration, IDbEntityTypeInfo<DbCategory> entityTypeInfo)
      : base(configuration, entityTypeInfo)
      {
      }

      public async Task<IEnumerable<DbCategory>> GetAllAsync(int? limit = 30)
      {
         var query = new Query(this.EntityTypeInfo.Name);

         if (limit != null) query = query.Limit((int)limit);

         var dbResult = await this.GetManyAsync(query);

         return dbResult;
      }
   }
}