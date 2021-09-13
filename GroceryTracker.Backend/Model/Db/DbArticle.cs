using System.Collections.Generic;

namespace GroceryTracker.Backend.Model.Db
{
   public record DbArticle
   {
      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }

      public double Pfand { get; set; }

      public string Details { get; set; }

      public List<string> Tags { get; set; }

      public int CategoryId { get; set; }

      public int BrandId { get; set; }
   }
}