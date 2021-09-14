using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IMarketAccess : IAccessBase<DbMarket>
   {
      Task<IEnumerable<DbMarket>> GetAllAsync(int? limit = 30);
   }

   public class MarketAccess : AccessBase<DbMarket>, IMarketAccess
   {
      public MarketAccess(IDatabaseConfiguration configuration, IDbEntityTypeInfo<DbMarket> entityTypeInfo)
      : base(configuration, entityTypeInfo)
      {
      }

      public async Task<IEnumerable<DbMarket>> GetAllAsync(int? limit = 30)
      {
         var query = new Query(this.EntityTypeInfo.Name);

         if (limit != null) query = query.Limit((int)limit);

         var dbResult = await this.GetManyAsync(query);

         return dbResult;
      }
   }
}