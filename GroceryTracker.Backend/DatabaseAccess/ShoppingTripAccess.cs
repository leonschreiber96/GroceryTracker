using System;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using GroceryTracker.Backend.Model.Dto;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IShoppingTripAccess : IAccessBase<DbShoppingTrip>
   {
      Task<ShoppingTripDto> GetSingleDetailed(int id);
   }

   public class ShoppingTripAccess : AccessBase<DbShoppingTrip>, IShoppingTripAccess
   {
      private DbEntityTypeInfo<DbArticle> articleEntityTypeInfo = new DbEntityTypeInfo<DbArticle>();
      private DbEntityTypeInfo<DbMarket> marketEntityTypeInfo = new DbEntityTypeInfo<DbMarket>();

      public ShoppingTripAccess(DatabaseConfiguration configuration) : base(configuration, new DbEntityTypeInfo<DbShoppingTrip>())
      {
      }

      public async Task<ShoppingTripDto> GetSingleDetailed(int id)
      {
         throw new NotImplementedException();
      }
   }
}