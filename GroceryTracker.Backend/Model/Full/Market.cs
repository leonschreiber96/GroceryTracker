using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Full
{
   public record Market
   {
      public Market(DbMarket dbObject)
      {
         this.Id = dbObject.Id;
         this.OwnerId = dbObject.OwnerId;
         this.Name = dbObject.Name;
      }

      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }
   }
}