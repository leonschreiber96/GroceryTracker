using System;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IShoppingTripAccess : IAccessBase<DbShoppingTrip>
   {

   }

   public class ShoppingTripAccess : AccessBase<DbShoppingTrip>, IShoppingTripAccess
   {
      public ShoppingTripAccess(DatabaseConfiguration configuration) : base(configuration, new DbEntityTypeInfo<DbShoppingTrip>())
      {
      }

      public ShoppingTripDto GetSingleDetailed(int id)
      {
         throw new NotImplementedException();
      }
   }
}