using System;

namespace GroceryTracker.Backend.Model.Dto
{
   public class ShoppingTripMetaDto
   {
      public int TripId { get; set; }

      public int OwnerId { get; set; }

      public DateTime Timestamp { get; set; }

      public int MarketId { get; set; }
   }
}