using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IBrandAccess : IAccessBase<DbBrand>
   {
      Task<IEnumerable<DbBrand>> GetAllAsync(int? limit = 30);
   }

   public class BrandAccess : AccessBase<DbBrand>, IBrandAccess
   {
      public BrandAccess(IDatabaseConfiguration configuration, IDbEntityTypeInfo<DbBrand> entityTypeInfo)
      : base(configuration, entityTypeInfo)
      {
      }

      public async Task<IEnumerable<DbBrand>> GetAllAsync(int? limit = 30)
      {
         var query = new Query(this.EntityTypeInfo.Name);

         if (limit != null) query = query.Limit((int)limit);

         var dbResult = await this.GetManyAsync(query);

         return dbResult;
      }
   }
}