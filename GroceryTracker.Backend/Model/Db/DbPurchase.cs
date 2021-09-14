using GroceryTracker.Backend.DatabaseAccess.Enum;

namespace GroceryTracker.Backend.Model.Db
{
   public record DbPurchase
   {
      /// <summary>
      /// Id of the shoppping trip this purchase was part of.
      /// </summary>
      public int TripId { get; set; }

      /// <summary>
      /// Id of the purchased article.
      /// </summary>
      public int ArticleId { get; set; }

      /// <summary>
      /// Price of one unit of the purchased article (e.g. 1 piece of bread, 1 kg of bananas).
      /// </summary>
      public double PaymentUnitPrice { get; set; }

      /// <summary>
      /// Amount of units (piece, kg, litre).
      /// </summary>
      public double PaymentUnitAmount { get; set; }

      /// <summary>
      /// The unit in which the article was counted and that was used to determine the price.
      /// </summary>
      public Unit PaymentUnit { get; set; }

      /// <summary>
      /// The physical "mass" of the bought article. Only relevant for piecewise articles since they have a mass
      /// (e.g. 1 milk carton = 1000ml) that is not obvious from the UnitAmount.
      /// </summary>
      public double PhysicalAmount { get; set; }

      /// <summary>
      /// The unit that can be used to determine the physical "mass" of the bought article
      /// (e.g. 1 carton of milk (<see cref="member">PaymentUnit</see> = <see>Unit.Piece</see>) is 1000ml).
      /// </summary>
      public Unit PhysicalUnit { get; set; }
   }
}