using System;
using System.Collections.Generic;
using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Full
{
   public record ShoppingTrip
   {
      public ShoppingTrip(DbShoppingTrip dbObject, Market market, IEnumerable<Purchase> purchases)
      {
         this.Id = dbObject.Id;
         this.OwnerId = dbObject.OwnerId;
         this.Timestamp = dbObject.Timestamp;
         this.Market = market;
         this.Purchases = purchases;
      }

      public int Id { get; set; }

      public int OwnerId { get; set; }

      public DateTime Timestamp { get; set; }

      public Market Market { get; set; }

      public IEnumerable<Purchase> Purchases { get; set; }
   }
}