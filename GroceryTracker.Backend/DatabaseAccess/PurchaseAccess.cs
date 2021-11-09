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

      Task<IEnumerable<RecentPurchaseOverviewDto>> GetRecentAsync(int marketId, int limit = 30);

      Task<IEnumerable<FrequentPurchaseOverviewDto>> GetFrequentAsync(int marketId, int limit = 30);
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

      public async Task<IEnumerable<RecentPurchaseOverviewDto>> GetRecentAsync(int marketId, int limit = 30)
      {
         var selectColumns = new string[]
         {
            this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Timestamp)),
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Name)) + " as ArticleName",
            this.brandEtInfo.FullPropPath(nameof(DbBrand.Name)) + " as BrandName",
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Details))
         };

         var query = new Query(this.EntityTypeInfo.Name)
            .Select(selectColumns)
            .Join(this.tripEtInfo.Name, this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbPurchase.TripId)))
            .Where(this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.MarketId)), marketId)
            .OrderByDesc(this.tripEtInfo.FullPropPath(nameof(ShoppingTrip.Timestamp)))
            .Limit(limit)
            .Join(this.articleEtInfo.Name, this.articleEtInfo.FullPropPath(nameof(DbArticle.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbPurchase.ArticleId)))
            .LeftJoin(this.brandEtInfo.Name, this.brandEtInfo.FullPropPath(nameof(DbBrand.Id)), this.articleEtInfo.FullPropPath(nameof(DbArticle.BrandId)));

         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var dbResult = await queryFactory.FromQuery(query).GetAsync<RecentPurchaseOverviewDto>();

            return dbResult;
         }
      }

      public async Task<IEnumerable<FrequentPurchaseOverviewDto>> GetFrequentAsync(int marketId, int limit = 30)
      {
         var selectColumns = new string[]
         {
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Name)) + " as ArticleName",
            this.brandEtInfo.FullPropPath(nameof(DbBrand.Name)) + " as BrandName",
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Details))
         };

         var subQuery = new Query(this.EntityTypeInfo.Name)
            .Join(this.tripEtInfo.Name, this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbPurchase.TripId)))
            .Where(this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.MarketId)), marketId);

         var query = new Query()
            .From(subQuery, "x")
            .SelectRaw($"count ({this.articleEtInfo.FullPropPath(nameof(DbArticle.Id))}) as PurchaseCount")
            .Select(selectColumns)
            .Join(this.articleEtInfo.Name, "x." + this.EntityTypeInfo.Props[nameof(DbPurchase.ArticleId)], this.articleEtInfo.FullPropPath(nameof(DbArticle.Id)))
            .LeftJoin(this.brandEtInfo.Name, this.articleEtInfo.FullPropPath(nameof(DbArticle.BrandId)), this.brandEtInfo.FullPropPath(nameof(DbBrand.Id)))
            .GroupBy(this.articleEtInfo.FullPropPath(nameof(DbArticle.Id)), this.brandEtInfo.FullPropPath(nameof(DbBrand.Id)))
            .OrderByRaw("PurchaseCount DESC")
            .Limit(30);

         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            Console.WriteLine(queryFactory.Compiler.Compile(query).RawSql);
            var dbResult = await queryFactory.FromQuery(query).GetAsync<FrequentPurchaseOverviewDto>();

            return dbResult;
         }

         throw new NotImplementedException();
      }
   }
}