using System;
using System.Text.Json.Serialization;
using GroceryTracker.Backend.Auth;
using GroceryTracker.Backend.DatabaseAccess;
using GroceryTracker.Backend.Model.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GroceryTracker.Backend
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddControllers().AddJsonOptions(options =>
         {
            var enumConverter = new JsonStringEnumConverter();
            options.JsonSerializerOptions.Converters.Add(enumConverter);
         });
         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "GroceryTracker.Backend", Version = "v1" });
         });

         Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
         var dbConfig = Configuration.GetSection("Database").Get<DatabaseConfiguration>();
         var identityKey = Configuration["Identity:Key"];

         services.AddSingleton<ISessionManager>(x => new SessionManager(new TimeSpan(hours: 0, minutes: 20, seconds: 0)));

         services.AddSingleton<IDatabaseConfiguration>(dbConfig);

         services.AddSingleton<IDbEntityTypeInfo<DbAppUser>, DbEntityTypeInfo<DbAppUser>>();
         services.AddSingleton<IDbEntityTypeInfo<DbArticle>, DbEntityTypeInfo<DbArticle>>();
         services.AddSingleton<IDbEntityTypeInfo<DbBrand>, DbEntityTypeInfo<DbBrand>>();
         services.AddSingleton<IDbEntityTypeInfo<DbCategory>, DbEntityTypeInfo<DbCategory>>();
         services.AddSingleton<IDbEntityTypeInfo<DbMarket>, DbEntityTypeInfo<DbMarket>>();
         services.AddSingleton<IDbEntityTypeInfo<DbPurchase>, DbEntityTypeInfo<DbPurchase>>();
         services.AddSingleton<IDbEntityTypeInfo<DbShoppingTrip>, DbEntityTypeInfo<DbShoppingTrip>>();

         services.AddSingleton<IUserAccess, AppUserAccess>();
         services.AddSingleton<IArticleAccess, ArticleAccess>();
         services.AddSingleton<IBrandAccess, BrandAccess>();
         services.AddSingleton<ICategoryAccess, CategoryAccess>();
         services.AddSingleton<IMarketAccess, MarketAccess>();
         services.AddSingleton<IPurchaseAccess, PurchaseAccess>();
         services.AddSingleton<IShoppingTripAccess, ShoppingTripAccess>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GroceryTracker.Backend v1"));
         }

         app.UseHttpsRedirection();

         app.UseRouting();

         app.UseEndpoints(endpoints =>
         {
            endpoints
            .MapControllers();
            // .RequireAuthorization();
         });
      }
   }
}
