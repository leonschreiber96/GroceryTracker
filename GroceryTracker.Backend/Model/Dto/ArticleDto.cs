using System.Collections.Generic;

namespace GroceryTracker.Backend.Model.Dto
{
   public class ArticleDto
   {
      public int ArticleId { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }

      public string Details { get; set; }

      public double? Pfand { get; set; }

      public string[] Tags { get; set; }

      public int CategoryId { get; set; }

      public int? BrandId { get; set; }
   }
}