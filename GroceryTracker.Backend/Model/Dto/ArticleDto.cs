using System.Collections.Generic;

namespace GroceryTracker.Backend.Model.Dto
{
   public class ArticleDto
   {
      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }

      public string Details { get; set; }

      public List<string> Tags { get; set; }
   }
}