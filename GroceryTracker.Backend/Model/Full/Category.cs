using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Full
{
   public record Category
   {
      public Category(DbCategory dbObject, Category parent)
      {
         this.Id = dbObject.Id;
         this.Name = dbObject.Name;
         this.OwnerId = dbObject.OwnerId;
         this.Parent = parent;
      }

      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }

      public Category Parent { get; set; }
   }
}