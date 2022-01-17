using System;
using System.Diagnostics;
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
      public async Task<IActionResult> Post([FromBody] string searchString, [FromQuery] int limit = 20)
      {
         System.Console.WriteLine("[SearchController.Post] Received search string: " + searchString);
         var sw = new Stopwatch();
         sw.Start();

         try
         {
            var paramRegex = new Regex(@"((?<key>art|det|bra|cat)=(?<value>\S+))");
            var search = new ArticleSearch { OriginalSearchString = searchString };

            var paramMatches = paramRegex.Matches(searchString);

            search.ArticleName = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "art")?.Groups["value"].Value;
            search.BrandName = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "bra")?.Groups["value"].Value;
            search.Details = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "det")?.Groups["value"].Value;
            search.PrimaryCategory = paramMatches.FirstOrDefault(x => x.Groups["key"].Value == "cat")?.Groups["value"].Value;

            search.DynamicSearchString = paramRegex.Replace(searchString, "").Trim();

            search.ResultLimit = limit;

            var returnValue = await articleAccess.SearchArticle(search);
            returnValue.Search = search;

            return StatusCode(200, returnValue);
         }
         catch (Exception ex) when (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
         {
            return StatusCode(501, ex.Message);
         }
         finally
         {
            sw.Stop();
            System.Console.WriteLine("[SearchController.Post] Search took " + sw.ElapsedMilliseconds + "ms");
         }

      }
   }
}
