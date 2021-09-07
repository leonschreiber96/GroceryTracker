using System;
using System.Collections.Generic;

namespace GroceryTracker.Backend.Model.Db
{
   public record DbShoppingTrip
   {
      public int Id { get; set; }

      public int OwnerId { get; set; }

      public DateTime Timestamp { get; set; }

      public int MarketId { get; set; }
   }
}