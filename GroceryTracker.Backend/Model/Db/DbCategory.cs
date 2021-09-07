namespace GroceryTracker.Backend.Model.Db
{
   public class DbCategory
   {
      public int Id { get; set; }

      public int OwnerId { get; set; }

      public string Name { get; set; }

      public int ParentId { get; set; }
   }
}