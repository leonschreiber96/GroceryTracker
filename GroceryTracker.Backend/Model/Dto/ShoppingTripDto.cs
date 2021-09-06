using System;
using System.Collections.Generic;

public class ShoppingTripDto
{
   public int Id { get; set; }

   public int OwnerId { get; set; }

   public DateTime Timestamp { get; set; }

   public int Market { get; set; }

   public List<PurchaseDto> Purchases { get; set; }
}