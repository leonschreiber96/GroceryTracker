using System;

namespace GroceryTracker.Backend.Model.Dto
{
   public class PurchaseOverviewDto
   {
      public int ArticleId { get; set; }

      public string ArticleName { get; set; }

      public string BrandName { get; set; }

      public string Details { get; set; }
   }
}