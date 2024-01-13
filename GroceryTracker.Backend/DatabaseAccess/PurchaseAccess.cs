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
      /// <summary>
      /// Returns all purchases
      /// </summary>
      /// <param name="limit">The number of purchases to return at max</param>
      Task<IEnumerable<DbPurchase>> GetAllAsync(int limit = 30);

      /// <summary>
      /// Returns the last purchases for the given market.
      /// </summary>
      /// <param name="marketId">Id of the market</param>
      /// <param name="limit">How many purchases to return at max</param>
      Task<IEnumerable<PurchaseOverviewDto>> GetRecentAsync(int marketId, int limit = 30);

      /// <summary>
      /// Returns the most frequently bought articles for the given market.
      /// </summary>
      /// <param name="marketId">Id of the market</param>
      /// <param name="limit">How many purchases to return at max</param>
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

      public async Task<IEnumerable<PurchaseOverviewDto>> GetRecentAsync(int marketId, int limit = 30)
      {
         var selectColumns = new string[]
         {
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Id)) + " as ArticleId",
            this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Timestamp)),
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Name)) + " as ArticleName",
            this.brandEtInfo.FullPropPath(nameof(DbBrand.Name)) + " as BrandName",
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Details))
         };

         /*
         SELECT "article"."id" AS "ArticleId", "shopping_trip"."timestamp", "article"."name" AS "ArticleName", "brand"."name" AS "BrandName", "article"."details" FROM "purchase" 
         INNER JOIN "shopping_trip" ON "shopping_trip"."id" = "purchase"."trip_id"
         INNER JOIN "article" ON "article"."id" = "purchase"."article_id"
         LEFT JOIN "brand" ON "brand"."id" = "article"."brand_id" 
         WHERE "shopping_trip"."market_id" = [marketId] 
         ORDER BY "shopping_trip"."timestamp" DESC 
         LIMIT [limit]
         */
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
            var dbResult = await queryFactory.FromQuery(query).GetAsync<PurchaseOverviewDto>();

            return dbResult;
         }
      }

      public async Task<IEnumerable<PurchaseOverviewDto>> GetFrequentAsync(int marketId, int limit = 30)
      {
         var selectColumns = new string[]
         {
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Id)) + " as ArticleId",
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Name)) + " as ArticleName",
            this.brandEtInfo.FullPropPath(nameof(DbBrand.Name)) + " as BrandName",
            this.articleEtInfo.FullPropPath(nameof(DbArticle.Details))
         };

         /*
         SELECT * FROM "purchase" 
         INNER JOIN "shopping_trip" ON "shopping_trip"."id" = "purchase"."trip_id" 
         WHERE "shopping_trip"."market_id" = ?
         */
         var subQuery = new Query(this.EntityTypeInfo.Name)
            .Join(this.tripEtInfo.Name, this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbPurchase.TripId)))
            .Where(this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.MarketId)), marketId);

         /*
         SELECT count (article.id) as PurchaseCount, "article"."id" AS "ArticleId", "article"."name" AS "ArticleName", "brand"."name" AS "BrandName", "article"."details" 
         FROM (
            SELECT * FROM "purchase" 
            INNER JOIN "shopping_trip" ON "shopping_trip"."id" = "purchase"."trip_id" 
            WHERE "shopping_trip"."market_id" = [marketId]
         ) AS "x" 
         INNER JOIN "article" ON "x"."article_id" = "article"."id"
         LEFT JOIN "brand" ON "article"."brand_id" = "brand"."id" 
         GROUP BY "article"."id", "brand"."id" 
         ORDER BY PurchaseCount DESC 
         LIMIT [limit]
         */
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
            var dbResult = await queryFactory.FromQuery(query).GetAsync<PurchaseOverviewDto>();

            return dbResult;
         }
      }
   }
}