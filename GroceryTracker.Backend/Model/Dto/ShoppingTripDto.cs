using System;
using System.Collections.Generic;

namespace GroceryTracker.Backend.Model.Dto
{
   public class ShoppingTripDto
   {
      public int TripId { get; set; }

      public int OwnerId { get; set; }

      public DateTime Timestamp { get; set; }

      public int Market { get; set; }

      public List<PurchaseDto> Purchases { get; set; }
   }
}