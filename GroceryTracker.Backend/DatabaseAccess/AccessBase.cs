using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

public interface IAccessBase<T>
{
   Task<T> GetSingleAsync(int id);

   Task<List<T>> GetManyAsync(int ids);

   Task Upsert(T newValue);
}

public abstract class AccessBase<T> : IAccessBase<T>
{
   private string ConnectionString { get; }

   protected DbEntityTypeInfo<T> EntityInfo { get; }

   protected AccessBase(string host, int port, string databaseName, string username, string password)
   {
      this.ConnectionString = $"Server={host};Port={port};Database={databaseName};User Id={username};Password={password};";
      this.EntityInfo = new DbEntityTypeInfo<T>();
   }

   public async Task<T> GetSingleAsync(int id)
   {
      using (var connection = this.CreateConnection())
      {
         var sql = $"SELECT * FROM {this.EntityInfo.Name} WHERE id = @id";
         var dbResult = await connection.QuerySingleAsync<T>(sql, new { id });

         return dbResult;
      }
   }

   public async Task<List<T>> GetManyAsync(int ids)
   {
      using (var connection = this.CreateConnection())
      {
         var sql = $"SELECT * FROM {this.EntityInfo.Name} WHERE id IN @ids";
         var dbResult = await connection.QueryAsync<T>(sql, new { ids });

         return dbResult.ToList();
      }
   }

   public async Task Upsert(T newValue)
   {
      using (var connection = this.CreateConnection())
      {
         var values = this.EntityInfo.GetValuePairs(newValue);
         var sql = $"INSERT OR UPDATE INTO ${this.EntityInfo.Name} ({string.Join(',', values.Keys)}) VALUES (${string.Join(',', values.Values)})";

         await connection.ExecuteAsync(sql);
      }
   }

   protected NpgsqlConnection CreateConnection() => new NpgsqlConnection(this.ConnectionString);
}