namespace GroceryTracker.Backend.Model.Dto
{
   public class CategoryDto
   {
      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }

      public int Parent { get; set; }
   }
}