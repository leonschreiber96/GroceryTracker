
namespace GroceryTracker.Backend.Model.Db
{
   public record DbMarket
   {
      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }
   }
}