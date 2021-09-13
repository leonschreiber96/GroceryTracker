namespace GroceryTracker.Backend.Model.Db
{
   public record DbBrand
   {
      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }
   }
}