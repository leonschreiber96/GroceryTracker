using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using GroceryTracker.Backend.Search;
using SqlKata;
using SqlKata.Execution;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IArticleAccess : IAccessBase<DbArticle>
   {
      Task<SearchResultsDto> SearchArticle(ArticleSearch searchParams);
   }

   public class ArticleAccess : AccessBase<DbArticle>, IArticleAccess
   {
      private readonly IDbEntityTypeInfo<DbBrand> brandEtInfo;
      private readonly IDbEntityTypeInfo<DbCategory> categoryEtInfo;
      private readonly IDbEntityTypeInfo<DbPurchase> purchaseEtInfo;
      private readonly IDbEntityTypeInfo<DbShoppingTrip> tripEtInfo;

      public ArticleAccess(IDatabaseConfiguration configuration,
         IDbEntityTypeInfo<DbArticle> entityTypeInfo,
         IDbEntityTypeInfo<DbBrand> brandEtInfo,
         IDbEntityTypeInfo<DbCategory> categoryEtInfo,
         IDbEntityTypeInfo<DbPurchase> purchaseEtInfo,
         IDbEntityTypeInfo<DbShoppingTrip> tripEtInfo)
      : base(configuration, entityTypeInfo)
      {
         this.brandEtInfo = brandEtInfo;
         this.categoryEtInfo = categoryEtInfo;
         this.purchaseEtInfo = purchaseEtInfo;
         this.tripEtInfo = tripEtInfo;
      }

      public async Task<SearchResultsDto> SearchArticle(ArticleSearch searchParams)
      {
         var articleSearchQuery = new Query(this.EntityTypeInfo.Name)
            .Where(this.EntityTypeInfo.FullPropPath(nameof(DbArticle.Name)), "LIKE", $"%{searchParams.DynamicSearchString}%");

         var articleInfoJoinQuery = new Query()
            .Select(this.EntityTypeInfo.FullPropPath(nameof(DbArticle.Id)))
            .Select(this.EntityTypeInfo.FullPropPath(nameof(DbArticle.Details)) + " AS " + nameof(SearchResultDto.Details))
            .Select(this.EntityTypeInfo.FullPropPath(nameof(DbArticle.Name)) + " AS " + nameof(SearchResultDto.ArticleName))
            .Select(this.brandEtInfo.FullPropPath(nameof(DbBrand.Name)) + " AS " + (nameof(SearchResultDto.BrandName)))
            .Select(this.categoryEtInfo.FullPropPath(nameof(DbCategory.Name)) + " AS " + (nameof(SearchResultDto.CategoryName)))
            .From(articleSearchQuery.As(this.EntityTypeInfo.Name))
            .Join(this.brandEtInfo.Name, this.brandEtInfo.FullPropPath(nameof(DbBrand.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbArticle.BrandId)))
            .Join(this.categoryEtInfo.Name, this.categoryEtInfo.FullPropPath(nameof(DbCategory.Id)), this.EntityTypeInfo.FullPropPath(nameof(DbArticle.CategoryId)));

         var lastPriceQuery = new Query(this.purchaseEtInfo.Name)
            .Select(this.purchaseEtInfo.FullPropPath(nameof(DbPurchase.PaymentUnitPrice)))
            .Join(this.tripEtInfo.Name, this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Id)), this.purchaseEtInfo.FullPropPath(nameof(DbPurchase.TripId)))
            .WhereColumns(this.EntityTypeInfo.FullPropPath(nameof(DbArticle.Id)), "=", this.purchaseEtInfo.FullPropPath(nameof(DbPurchase.ArticleId)))
            .OrderByDesc(this.tripEtInfo.FullPropPath(nameof(DbShoppingTrip.Timestamp)))
            .Limit(1);

         var pricesQuery = new Query(this.purchaseEtInfo.Name)
            .SelectRaw($"\"{this.EntityTypeInfo.Name}\".\"{this.EntityTypeInfo.Props[nameof(DbArticle.Id)]}\", (sum(\"{this.purchaseEtInfo.Name}\".\"{this.purchaseEtInfo.Props[nameof(DbPurchase.PaymentUnitPrice)]}\") / count(*))::numeric(10,2) AS \"{nameof(SearchResultDto.AveragePrice)}\"")
            .Select(lastPriceQuery, nameof(SearchResultDto.LastPrice))
            .LeftJoin(this.EntityTypeInfo.Name, this.EntityTypeInfo.FullPropPath(nameof(DbArticle.Id)), this.purchaseEtInfo.FullPropPath(nameof(DbPurchase.ArticleId)))
            .GroupBy(this.EntityTypeInfo.FullPropPath(nameof(DbArticle.Id)));

         var query = new Query()
            .Select(nameof(SearchResultDto.ArticleName), nameof(SearchResultDto.Details), nameof(SearchResultDto.BrandName), nameof(SearchResultDto.CategoryName), nameof(SearchResultDto.AveragePrice), nameof(SearchResultDto.LastPrice))
            .From(articleInfoJoinQuery.As("articles"))
            .LeftJoin(pricesQuery.As("prices"), j => j.On($"prices.{this.EntityTypeInfo.Props[nameof(DbArticle.Id)]}", $"articles.{this.EntityTypeInfo.Props[nameof(DbArticle.Id)]}"));

         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var dbResult = await queryFactory.FromQuery(query)
               .GetAsync<SearchResultDto>();

            var result = new SearchResultsDto { Results = dbResult };

            return result;
         }
      }
   }
}