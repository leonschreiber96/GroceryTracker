using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IAccessBase<T>
   {
      Task<T> GetSingleAsync(int id);

      Task<T> GetSingleAsync(Query query);

      Task<IEnumerable<T>> GetManyAsync(int ids);

      Task<IEnumerable<T>> GetManyAsync(Query query);

      Task<IEnumerable<T>> GetAllAsync(int limit);

      Task Upsert(T newValue);

      Task Insert(T newValue);

      Task Update(T value);

      Task Delete(int id);
   }

   public abstract class AccessBase<T> : IAccessBase<T>
   {
      private string ConnectionString { get; }

      protected IDbEntityTypeInfo<T> EntityTypeInfo { get; }
      protected PostgresCompiler SqlCompiler { get; }

      protected AccessBase(string host, int port, string databaseName, string username, string password, IDbEntityTypeInfo<T> entityTypeInfo)
      {
         this.ConnectionString = $"Server={host};Port={port};Database={databaseName};User Id={username};Password={password};";
         this.EntityTypeInfo = entityTypeInfo;
         this.SqlCompiler = new PostgresCompiler();
      }

      protected AccessBase(DatabaseConfiguration configuration, IDbEntityTypeInfo<T> entityTypeInfo)
      {
         this.ConnectionString = $"Server={configuration.Hostname};Port={configuration.Port};Database={configuration.DatabaseName};User Id={configuration.Username};Password={configuration.Password};";
         this.EntityTypeInfo = entityTypeInfo;
         this.SqlCompiler = new PostgresCompiler();
      }

      public async Task<T> GetSingleAsync(int id)
      {
         using (var connection = this.CreateConnection())
         {
            var sql = $"SELECT * FROM {this.EntityTypeInfo.Name} WHERE id = {id}";
            var dbResult = await connection.QuerySingleOrDefaultAsync<T>(sql);

            return dbResult;
         }
      }


      public async Task<T> GetSingleAsync(Query query)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var dbResult = await queryFactory.FirstOrDefaultAsync<T>(query);

            return dbResult;
         }
      }

      public async Task<IEnumerable<T>> GetAllAsync(int limit)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var query = new Query(this.EntityTypeInfo.Name).Limit(limit).GetAsync<T>();
            var dbResult = await queryFactory.Query(this.EntityTypeInfo.Name).Limit(limit).GetAsync<T>();

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

      public async Task<IEnumerable<T>> GetManyAsync(Query query)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var dbResult = await queryFactory.GetAsync<T>(query);

            return dbResult;
         }
      }

      public async Task Insert(T newValue)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var values = new Dictionary<string, string>(
               this.EntityTypeInfo.GetValuePairs(newValue)
               .Where(x => !string.Equals(x.Key, "id", StringComparison.OrdinalIgnoreCase))
            );

            var fieldNames = values.Keys.ToList();
            var fieldValues = values.Values.Select(x => x ?? "").ToList();

            var sb = new StringBuilder($"INSERT INTO {this.EntityTypeInfo.Name} (");
            sb.Append(string.Join(',', fieldNames));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', fieldValues));
            sb.Append($");");

            var sql = sb.ToString();

            await connection.ExecuteAsync(sql, newValue);
         }
      }

      public async Task Update(T value)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var values = this.EntityTypeInfo.GetValuePairs(value);
            var fieldNames = values.Keys.ToList();
            var fieldValues = values.Values.Select(x => x ?? "").ToList();

            var sb = new StringBuilder($"UPDATE {this.EntityTypeInfo.Name} ");
            sb.Append($"SET {fieldNames[0]} = {fieldValues[0]}");

            for (int i = 1; i < fieldNames.Count; i++)
            {
               sb.Append($",{fieldNames[i]}  = {fieldValues[i]} ");
            }

            sb.Append($"WHERE {this.EntityTypeInfo.Name}.id = {values["id"]};");

            var sql = sb.ToString();

            await connection.ExecuteAsync(sql, value);
         }
      }

      public async Task Upsert(T newValue)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var values = this.EntityTypeInfo.GetValuePairs(newValue);
            var fieldNames = values.Keys.ToList();
            var fieldValues = values.Values.Select(x => x ?? "").ToList();

            var sb = new StringBuilder($"INSERT INTO {this.EntityTypeInfo.Name} (");
            sb.Append(string.Join(',', fieldNames));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', fieldValues));
            sb.Append($") ON CONFLICT (id) DO UPDATE ");
            sb.Append($"SET {fieldNames[0]} = {fieldValues[0]}");

            for (int i = 1; i < fieldNames.Count; i++)
            {
               sb.Append($",{fieldNames[i]}  = {fieldValues[i]} ");
            }

            sb.Append($"WHERE {this.EntityTypeInfo.Name}.id = {values["id"]};");

            var sql = sb.ToString();

            await connection.ExecuteAsync(sql, newValue);
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

      protected QueryFactory QueryFactory(NpgsqlConnection connection) => new QueryFactory(connection, this.SqlCompiler);
   }
}