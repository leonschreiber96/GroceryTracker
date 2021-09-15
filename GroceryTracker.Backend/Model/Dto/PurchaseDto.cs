using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Dto
{
   public class PurchaseDto
   {
      public int TripId { get; set; }

      public int ArticleId { get; set; }

      public double Quantity { get; set; }

      public double PricePerUnit { get; set; }

      public double Pfand { get; set; }

      public string UnitSize { get; set; }
   }
}