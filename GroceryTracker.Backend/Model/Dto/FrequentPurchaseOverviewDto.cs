using System;

namespace GroceryTracker.Backend.Model.Dto
{
   public class FrequentPurchaseOverviewDto
   {
      public int PurchaseCount { get; set; }

      public string ArticleName { get; set; }

      public string BrandName { get; set; }

      public string Details { get; set; }
   }
}