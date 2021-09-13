using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface ICategoryAccess : IAccessBase<DbCategory>
   {
   }

   public class CategoryAccess : AccessBase<DbCategory>, ICategoryAccess
   {
      public CategoryAccess(DatabaseConfiguration configuration) : base(configuration, new DbEntityTypeInfo<DbCategory>())
      {
      }
   }
}