using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IAccessBase<T>
   {
      Task<T> GetSingleAsync(int id);

      Task<T> GetSingleAsync(string sql);

      Task<IEnumerable<T>> GetManyAsync(int ids);

      Task<IEnumerable<T>> GetManyAsync(string sql);

      Task Upsert(T newValue);

      Task Delete(int id);
   }

   public abstract class AccessBase<T> : IAccessBase<T>
   {
      private string ConnectionString { get; }

      protected IDbEntityTypeInfo<T> EntityTypeInfo { get; }
      protected SqlServerCompiler SqlServerCompiler { get; }

      protected AccessBase(string host, int port, string databaseName, string username, string password, IDbEntityTypeInfo<T> entityTypeInfo)
      {
         this.ConnectionString = $"Server={host};Port={port};Database={databaseName};User Id={username};Password={password};";
         this.EntityTypeInfo = entityTypeInfo;
         this.SqlServerCompiler = new SqlServerCompiler();
      }

      public async Task<T> GetSingleAsync(int id)
      {
         using (var connection = this.CreateConnection())
         {
            var sql = $"SELECT * FROM {this.EntityTypeInfo.Name} WHERE id = ${id}";
            var dbResult = await connection.QuerySingleAsync<T>(sql);

            return dbResult;
         }
      }

      public async Task<T> GetSingleAsync(string sql)
      {
         using (var connection = this.CreateConnection())
         {
            var dbResult = await connection.QuerySingleOrDefaultAsync<T>(sql);

            return dbResult;
         }
      }

      public async Task<IEnumerable<T>> GetManyAsync(int ids)
      {
         using (var connection = this.CreateConnection())
         {
            var sql = $"SELECT * FROM {this.EntityTypeInfo.Name} WHERE id IN @ids";
            var dbResult = await connection.QueryAsync<T>(sql, new { ids });

            return dbResult;
         }
      }

      public async Task<IEnumerable<T>> GetManyAsync(string sql)
      {
         using (var connection = this.CreateConnection())
         {
            var dbResult = await connection.QueryAsync<T>(sql);

            return dbResult;
         }
      }

      public async Task Upsert(T newValue)
      {
         using (var connection = this.CreateConnection())
         {
            var values = this.EntityTypeInfo.GetValuePairs(newValue);
            var sql = $"INSERT OR UPDATE INTO {this.EntityTypeInfo.Name} ({string.Join(',', values.Keys)}) VALUES ({string.Join(',', values.Values)})";

            await connection.ExecuteAsync(sql);
         }
      }

      public async Task Delete(int id)
      {
         using (var connection = this.CreateConnection())
         {
            var sql = $"DELETE FROM {this.EntityTypeInfo.Name} WHERE id = {id}";

            await connection.ExecuteAsync(sql);
         }
      }

      protected NpgsqlConnection CreateConnection() => new NpgsqlConnection(this.ConnectionString);
   }
}