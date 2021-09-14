using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IShoppingTripAccess : IAccessBase<DbShoppingTrip>
   {
      Task<ShoppingTripDto> GetSingleDetailed(int id);

      Task<IEnumerable<DbShoppingTrip>> GetAllAsync(int? limit = 30);
   }

   public class ShoppingTripAccess : AccessBase<DbShoppingTrip>, IShoppingTripAccess
   {
      private IDbEntityTypeInfo<DbArticle> articleEntityTypeInfo;
      private IDbEntityTypeInfo<DbMarket> marketEntityTypeInfo;

      public ShoppingTripAccess(IDatabaseConfiguration configuration,
      IDbEntityTypeInfo<DbShoppingTrip> entityTypeInfo, IDbEntityTypeInfo<DbArticle> articleEntityTypeInfo, IDbEntityTypeInfo<DbMarket> marketEntityTypeInfo)
      : base(configuration, entityTypeInfo)
      {
         this.articleEntityTypeInfo = articleEntityTypeInfo;
         this.marketEntityTypeInfo = marketEntityTypeInfo;
      }

      public async Task<ShoppingTripDto> GetSingleDetailed(int id)
      {
         throw new NotImplementedException();
      }

      public async Task<IEnumerable<DbShoppingTrip>> GetAllAsync(int? limit = 30)
      {
         var query = new Query(this.EntityTypeInfo.Name);

         if (limit != null) query = query.Limit((int)limit);

         var dbResult = await this.GetManyAsync(query);

         return dbResult;
      }
   }
}