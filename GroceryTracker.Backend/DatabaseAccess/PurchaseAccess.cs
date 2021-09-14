using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IPurchaseAccess : IAccessBase<DbPurchase>
   {
      Task<PurchaseDto> GetSingleDetailed(int id);

      Task<IEnumerable<DbPurchase>> GetAllAsync(int? limit = 30);
   }

   public class PurchaseAccess : AccessBase<DbPurchase>, IPurchaseAccess
   {
      private readonly IDbEntityTypeInfo<DbArticle> articleEntityTypeInfo;
      private readonly IDbEntityTypeInfo<DbMarket> marketEntityTypeInfo;

      public PurchaseAccess(IDatabaseConfiguration configuration,
      IDbEntityTypeInfo<DbPurchase> entityTypeInfo, IDbEntityTypeInfo<DbArticle> articleEntityTypeInfo, IDbEntityTypeInfo<DbMarket> marketEntityTypeInfo)
      : base(configuration, entityTypeInfo)
      {
         this.articleEntityTypeInfo = articleEntityTypeInfo;
         this.marketEntityTypeInfo = marketEntityTypeInfo;
      }

      public async Task<PurchaseDto> GetSingleDetailed(int id)
      {
         throw new NotImplementedException();
      }

      public async Task<IEnumerable<DbPurchase>> GetAllAsync(int? limit = 30)
      {
         var query = new Query(this.EntityTypeInfo.Name);

         if (limit != null) query = query.Limit((int)limit);

         var dbResult = await this.GetManyAsync(query);

         return dbResult;
      }
   }
}