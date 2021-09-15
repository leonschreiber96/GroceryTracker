using System;
using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Dto
{
   public class PurchaseOverviewDto
   {
      DateTime Timestamp { get; set; }

      public string ArticleName { get; set; }

      public string BrandName { get; set; }

      public string Details { get; set; }
   }
}