using GroceryTracker.Backend.Model.Db;

namespace GroceryTracker.Backend.Model.Dto
{
   public record PurchaseDto : DbPurchase
   {
      public DbArticle Article { get; set; }

      public DbShoppingTrip Trip { get; set; }

      public DbCategory Category { get; set; }

      public DbBrand Brand { get; set; }
   }
}