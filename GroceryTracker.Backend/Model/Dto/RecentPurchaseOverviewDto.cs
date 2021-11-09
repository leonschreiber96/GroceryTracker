using System;

namespace GroceryTracker.Backend.Model.Dto
{
   public class RecentPurchaseOverviewDto
   {
      public DateTime Timestamp { get; set; }

      public string ArticleName { get; set; }

      public string BrandName { get; set; }

      public string Details { get; set; }
   }
}