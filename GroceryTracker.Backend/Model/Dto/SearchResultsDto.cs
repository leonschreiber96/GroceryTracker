
using System.Collections.Generic;
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
      public string ArticleName { get; set; }

      public string Details { get; set; }

      public string BrandName { get; set; }

      public string CategoryName { get; set; }

      public double LastPrice { get; set; }

      public double AveragePrice { get; set; }
   }
}