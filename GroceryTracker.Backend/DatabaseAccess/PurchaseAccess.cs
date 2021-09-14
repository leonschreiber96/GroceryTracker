using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using SqlKata;
using SqlKata.Execution;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IPurchaseAccess : IAccessBase<DbPurchase>
   {
      Task<IEnumerable<DbPurchase>> GetAllAsync(int? limit = 30);
   }

   public class PurchaseAccess : AccessBase<DbPurchase>, IPurchaseAccess
   {
      private readonly IDbEntityTypeInfo<DbArticle> articleEtInfo;
      private readonly IDbEntityTypeInfo<DbShoppingTrip> tripEtInfo;

      public PurchaseAccess(IDatabaseConfiguration configuration,
      IDbEntityTypeInfo<DbPurchase> etInfo, IDbEntityTypeInfo<DbArticle> articleEtInfo, IDbEntityTypeInfo<DbShoppingTrip> tripEtInfo)
      : base(configuration, etInfo)
      {
         this.articleEtInfo = articleEtInfo;
         this.tripEtInfo = tripEtInfo;
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