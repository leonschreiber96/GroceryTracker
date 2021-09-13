using System.Threading.Tasks;
using GroceryTracker.Backend.Model.Db;
using SqlKata;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IArticleAccess : IAccessBase<DbArticle>
   {

   }

   public class ArticleAccess : AccessBase<DbArticle>, IArticleAccess
   {
      public ArticleAccess(DatabaseConfiguration configuration) : base(configuration, new DbEntityTypeInfo<DbArticle>())
      {
      }
   }
}