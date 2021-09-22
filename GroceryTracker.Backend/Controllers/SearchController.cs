using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GroceryTracker.Backend.DatabaseAccess;
using GroceryTracker.Backend.Search;
using Microsoft.AspNetCore.Mvc;

namespace GroceryTracker.Backend.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class SearchController : ControllerBase
   {
      private readonly IArticleAccess articleAccess;

      public SearchController(IArticleAccess articleAccess)
      {
         this.articleAccess = articleAccess;
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody] string searchString)
      {
         try
         {
            var returnValue = new { };

            var paramRegex = new Regex(@"((?<key>art|det|bra|cat)=(?<value>\S+))");
            var search = new ArticleSearch { OriginalSearchString = searchString };

            var paramMatches = paramRegex.Matches(searchString);

            search.ArticleName = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "art")?.Value;
            search.BrandName = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "bra")?.Value;
            search.Details = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "det")?.Value;
            search.PrimaryCategory = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "cat")?.Value;

            search.DynamicSearchString = paramRegex.Replace(searchString, "").Trim();

            return StatusCode(200, returnValue);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }
      }
   }
}
