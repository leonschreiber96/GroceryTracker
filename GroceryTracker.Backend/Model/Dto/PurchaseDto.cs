public class PurchaseDto
{
   public int Id { get; set; }

   public int TripId { get; set; }

   public ArticleDto Article { get; set; }

   public ShoppingTripMetaDto ShoppingTrip { get; set; }

   public double Quantity { get; set; }

   public double PricePerUnit { get; set; }

   public double Pfand { get; set; }

   public string UnitSize { get; set; }
}