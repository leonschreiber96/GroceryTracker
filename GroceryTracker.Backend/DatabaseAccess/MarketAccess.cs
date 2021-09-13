using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IMarketAccess : IAccessBase<DbMarket>
   {
   }

   public class MarketAccess : AccessBase<DbMarket>, IMarketAccess
   {
      public MarketAccess(DatabaseConfiguration configuration) : base(configuration, new DbEntityTypeInfo<DbMarket>())
      {
      }
   }
}