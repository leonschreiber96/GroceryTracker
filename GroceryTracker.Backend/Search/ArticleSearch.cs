using System.Collections.Generic;

namespace GroceryTracker.Backend.Search
{
   public class ArticleSearch
   {
      public string OriginalSearchString { get; set; }

      public string ArticleName { get; set; }

      public string BrandName { get; set; }

      public string PrimaryCategory { get; set; }

      public string Details { get; set; }

      public string DynamicSearchString { get; set; }

      public int ResultLimit { get; set; }
   }
}