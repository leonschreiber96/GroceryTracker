using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IBrandAccess : IAccessBase<DbBrand>
   {
   }

   public class BrandAccess : AccessBase<DbBrand>, IBrandAccess
   {
      public BrandAccess(DatabaseConfiguration configuration) : base(configuration, new DbEntityTypeInfo<DbBrand>())
      {
      }
   }
}