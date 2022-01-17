using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using GroceryTracker.Backend.Search;

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
         var dbResult = await this.Function<SearchResultDto>(
            "search_article",
            searchParams.ArticleName,
            searchParams.BrandName,
            searchParams.PrimaryCategory,
            searchParams.Details,
            searchParams.DynamicSearchString,
            searchParams.ResultLimit);

         return new SearchResultsDto { Search = searchParams, Results = dbResult };
      }
   }
}