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
      public ArticleAccess(IDatabaseConfiguration configuration, IDbEntityTypeInfo<DbArticle> entityTypeInfo)
      : base(configuration, entityTypeInfo)
      {
      }
   }
}