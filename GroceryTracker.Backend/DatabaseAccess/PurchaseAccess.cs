using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using GroceryTracker.Backend.Model.Full;
using SqlKata;
using SqlKata.Execution;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IPurchaseAccess : IAccessBase<DbPurchase>
   {
      Task<IEnumerable<DbPurchase>> GetAllAsync(int limit = 30);

      Task<IEnumerable<PurchaseOverviewDto>> GetRecentAsync(int marketId, int limit = 30);

      Task<IEnumerable<PurchaseOverviewDto>> GetFrequentAsync(int marketId, int limit = 30);
   }

   public class PurchaseAccess : AccessBase<DbPurchase>, IPurchaseAccess
   {
      private readonly IDbEntityTypeInfo<DbArticle> articleEtInfo;
      private readonly IDbEntityTypeInfo<DbShoppingTrip> tripEtInfo;
      private readonly IDbEntityTypeInfo<DbBrand> brandEtInfo;

      public PurchaseAccess(IDatabaseConfiguration configuration,
      IDbEntityTypeInfo<DbPurchase> etInfo,
      IDbEntityTypeInfo<DbArticle> articleEtInfo,
      IDbEntityTypeInfo<DbShoppingTrip> tripEtInfo,
      IDbEntityTypeInfo<DbBrand> brandEtInfo)
      : base(configuration, etInfo)
      {
         this.articleEtInfo = articleEtInfo;
         this.tripEtInfo = tripEtInfo;
         this.brandEtInfo = brandEtInfo;
      }

      public async Task<IEnumerable<DbPurchase>> GetAllAsync(int limit = 30)
      {
         var query = new Query(this.EntityTypeInfo.Name).Limit(limit);

         var dbResult = await this.GetManyAsync(query);

         return dbResult;
      }

      public async Task<IEnumerable<PurchaseOverviewDto>> GetFrequentAsync(int marketId, int limit = 30)
      {
         var selectColumns = new string[]
         {
            this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Timestamp)),
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Name)),
            this.brandEtInfo.FullPropPath(nameof(DbBrand.Name)),
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Details))
         };

         var query = new Query(this.EntityTypeInfo.Name)
            .Select(selectColumns)
            .Join(this.tripEtInfo.Name, this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbPurchase.TripId)))
            .Where(this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.MarketId)), marketId)
            .OrderByDesc(this.tripEtInfo.FullPropPath(nameof(ShoppingTrip.Timestamp)))
            .Limit(limit)
            .Join(this.articleEtInfo.Name, this.articleEtInfo.FullPropPath(nameof(DbArticle.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbPurchase.ArticleId)))
            .Join(this.brandEtInfo.Name, this.brandEtInfo.FullPropPath(nameof(DbBrand.Id)), this.articleEtInfo.FullPropPath(nameof(DbArticle.BrandId)));

         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var dbResult = await queryFactory.FromQuery(query).GetAsync<PurchaseOverviewDto>();

            return dbResult;
         }
      }

      public async Task<IEnumerable<PurchaseOverviewDto>> GetRecentAsync(int marketId, int limit = 30)
      {
         var selectColumns = new string[]
         {
            this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Timestamp)),
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Name)),
            this.brandEtInfo.FullPropPath(nameof(DbBrand.Name)),
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Details))
         };

         // select count(a.id), a.name, b.name from(select* from purchase p
         // join shopping_trip s on p.trip_id = s.id
         // where s.market_id = 1) x
         // join article a on x.article_id = a.id
         // left join brand b on a.brand_id = b.id
         // group by a.id, b.id
         // order by count desc
         // limit 4

         // using (var connection = this.CreateConnection())
         // using (var queryFactory = this.QueryFactory(connection))
         // {
         //    var dbResult = await queryFactory.FromQuery(query).GetAsync<PurchaseOverviewDto>();

         //    return dbResult;
         // }

         throw new NotImplementedException();
      }
   }
}