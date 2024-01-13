using System;
using System.Collections.Generic;
using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Dto
{
   public record ShoppingTripDto : DbShoppingTrip
   {
      public List<PurchaseDto> Purchases { get; set; }
   }
}