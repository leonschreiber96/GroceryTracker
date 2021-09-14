using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Full
{
   public record Brand
   {
      public Brand(DbBrand dbObject)
      {
         this.Id = dbObject.Id;
         this.Name = dbObject.Name;
         this.OwnerId = dbObject.OwnerId;
      }

      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }
   }
}