
using System.Collections.Generic;
using GroceryTracker.Backend.DatabaseAccess.Enum;
using GroceryTracker.Backend.Search;

namespace GroceryTracker.Backend.Model.Dto
{
   public class SearchResultsDto
   {
      public ArticleSearch Search { get; set; }
      public IEnumerable<SearchResultDto> Results { get; set; }
   }

   public class SearchResultDto
   {
      public int Id { get; set; }

      public string ArticleName { get; set; }

      public string Details { get; set; }

      public string BrandName { get; set; }

      public string CategoryName { get; set; }

      public SearchMatchType ArticleMatch { get; set; }

      public SearchMatchType DetailsMatch { get; set; }

      public SearchMatchType BrandMatch { get; set; }

      public SearchMatchType CategoryMatch { get; set; }

      public double LastPrice { get; set; }

      public double AveragePrice { get; set; }
   }
}