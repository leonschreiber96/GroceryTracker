using System.Collections.Generic;
using System.Linq;
using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Full
{
   public record Article
   {
      public Article(DbArticle dbObject, Category category, Brand brand)
      {
         this.Id = dbObject.Id;
         this.Name = dbObject.Name;
         this.Pfand = dbObject.Pfand;
         this.Details = dbObject.Details;
         this.Tags = dbObject.Tags.ToList();
         this.OwnerId = dbObject.OwnerId;
         this.Category = category;
         this.Brand = brand;
      }

      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }

      public double? Pfand { get; set; }

      public string Details { get; set; }

      public List<string> Tags { get; set; }

      public Category Category { get; set; }

      public Brand Brand { get; set; }
   }
}