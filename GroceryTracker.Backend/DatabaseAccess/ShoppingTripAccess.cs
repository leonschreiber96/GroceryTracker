using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using GroceryTracker.Backend.Model.Full;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IShoppingTripAccess : IAccessBase<DbShoppingTrip>
   {
      Task<ShoppingTrip> GetSingleDetailed(int id);

      Task<IEnumerable<DbShoppingTrip>> GetAllAsync(int? limit = 30);
   }

   public class ShoppingTripAccess : AccessBase<DbShoppingTrip>, IShoppingTripAccess
   {
      private readonly IDbEntityTypeInfo<DbArticle> articleEtInfo;
      private readonly IDbEntityTypeInfo<DbMarket> marketEtInfo;
      private readonly IDbEntityTypeInfo<DbPurchase> purchaseEtInfo;
      private readonly IDbEntityTypeInfo<DbBrand> brandEtInfo;
      private readonly IDbEntityTypeInfo<DbCategory> categoryEtInfo;


      public ShoppingTripAccess(IDatabaseConfiguration configuration,
      IDbEntityTypeInfo<DbShoppingTrip> etInfo,
      IDbEntityTypeInfo<DbArticle> articleEtInfo,
      IDbEntityTypeInfo<DbMarket> marketEtInfo,
      IDbEntityTypeInfo<DbPurchase> purchaseEtInfo,
      IDbEntityTypeInfo<DbBrand> brandEtInfo,
      IDbEntityTypeInfo<DbCategory> categoryEtInfo)
      : base(configuration, etInfo)
      {
         this.articleEtInfo = articleEtInfo;
         this.marketEtInfo = marketEtInfo;
         this.purchaseEtInfo = purchaseEtInfo;
         this.brandEtInfo = brandEtInfo;
         this.categoryEtInfo = categoryEtInfo;
      }

      public async Task<ShoppingTrip> GetSingleDetailed(int id)
      {
         // Join all necessary information for detailed info on each purchase
         var query = new Query(this.EntityTypeInfo.Name)
            .Join(this.marketEtInfo.Name, this.marketEtInfo.FullPropPath(nameof(DbMarket.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbShoppingTrip.MarketId)))
            .Join(this.purchaseEtInfo.Name, this.purchaseEtInfo.FullPropPath(nameof(DbPurchase.TripId)), this.EntityTypeInfo.FullPropPath(nameof(DbShoppingTrip.Id)))
            .Join(this.articleEtInfo.Name, this.articleEtInfo.FullPropPath(nameof(DbArticle.Id)), this.purchaseEtInfo.FullPropPath(nameof(DbPurchase.ArticleId)))
            .Join(this.brandEtInfo.Name, this.brandEtInfo.FullPropPath(nameof(DbBrand.Id)), this.articleEtInfo.FullPropPath(nameof(DbArticle.BrandId)))
            .Join(this.categoryEtInfo.Name, this.categoryEtInfo.FullPropPath(nameof(DbCategory.Id)), this.articleEtInfo.FullPropPath(nameof(DbArticle.CategoryId)))
            .Where(this.EntityTypeInfo.FullPropPath((nameof(DbShoppingTrip.Id))), id);

         var sql = this.SqlCompiler.Compile(query);

         // Mapper function that will be used by Dapper to extract a ShoopingTrip and Purchase from each row
         // -> Has to be flattened into one ShoppingTrip with a "Purchases" property afterwards
         (ShoppingTrip, Purchase) mapper(DbShoppingTrip dbTrip, DbMarket dbMarket, DbPurchase dbPurchase, DbArticle dbArticle, DbBrand dbBrand, DbCategory dbCategory)
         {
            var brand = new Brand(dbBrand);
            var category = new Category(dbCategory, null);
            var article = new Article(dbArticle, category, brand);
            var market = new Market(dbMarket);
            var trip = new ShoppingTrip(dbTrip, market, null);
            var purchase = new Purchase(dbPurchase, trip, article);

            return (trip, purchase);
         }

         using (var connection = this.CreateConnection())
         {
            var dbResult = await connection.QueryAsync<DbShoppingTrip, DbMarket, DbPurchase, DbArticle, DbBrand, DbCategory, (ShoppingTrip Trip, Purchase Purchase)>("", mapper);

            if (dbResult.Count() == 0) return null;

            var trip = dbResult.First().Trip;
            trip.Purchases = dbResult.Select(x => x.Purchase);

            return trip;
         }
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