using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GroceryTracker.Backend.ExtensionMethods;
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

      Task<int> UpsertAsync(T newValue);

      Task<int> InsertAsync(T newValue);

      Task UpdateAsync(T value);

      Task DeleteAsync(int id);

      Task<bool> ExistsAsync(int id);
   }

   public abstract class AccessBase<T> : IAccessBase<T>
   {
      private string ConnectionString { get; }

      protected IDbEntityTypeInfo<T> EntityTypeInfo { get; }
      protected PostgresCompiler SqlCompiler { get; }

      protected AccessBase(IDatabaseConfiguration configuration, IDbEntityTypeInfo<T> entityTypeInfo)
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

      public async Task<int> InsertAsync(T newValue)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var values = new Dictionary<string, string>(
               this.EntityTypeInfo.GetStringValues(newValue)
               .Where(x => !string.Equals(x.Key, "id", StringComparison.OrdinalIgnoreCase))
            );

            var fieldNames = values.Keys.ToList();
            var fieldValues = values.Values.Select(x => x ?? "").ToList();

            var sb = new StringBuilder($"INSERT INTO {this.EntityTypeInfo.Name} (");
            sb.Append(string.Join(',', fieldNames));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', fieldValues));
            sb.Append($") RETURNING id;");

            var sql = sb.ToString();

            var newId = await connection.ExecuteScalarAsync<int>(sql, newValue);

            return newId;
         }
      }

      public async Task UpdateAsync(T value)
      {
         using (var connection = this.CreateConnection())
         using (var queryFactory = this.QueryFactory(connection))
         {
            var values = this.EntityTypeInfo.GetStringValues(value);
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

      public async Task<int> UpsertAsync(T newValue)
      {
         // using (var connection = this.CreateConnection())
         // using (var queryFactory = this.QueryFactory(connection))
         // {
         //    var values = this.EntityTypeInfo.GetStringValues(newValue);
         //    var fieldNames = values.Keys.ToList();
         //    var fieldValues = values.Values.Select(x => x ?? "").ToList();

         //    var sb = new StringBuilder($"INSERT INTO {this.EntityTypeInfo.Name} (");
         //    sb.Append(string.Join(',', fieldNames));
         //    sb.Append(") VALUES (");
         //    sb.Append(string.Join(',', fieldValues));
         //    sb.Append($") ON CONFLICT (id) DO UPDATE ");
         //    sb.Append($"SET {fieldNames[0]} = {fieldValues[0]}");

         //    for (int i = 1; i < fieldNames.Count; i++)
         //    {
         //       sb.Append($",{fieldNames[i]}  = {fieldValues[i]} ");
         //    }

         //    sb.Append($"WHERE {this.EntityTypeInfo.Name}.id = {values["id"]};");

         //    var sql = sb.ToString();

         //    await connection.ExecuteAsync(sql, newValue);
         // }

         var tcs = new TaskCompletionSource<int>();
         tcs.SetException(new NotImplementedException());
         return await tcs.Task;
      }

      public async Task DeleteAsync(int id)
      {
         using (var connection = this.CreateConnection())
         {
            var sql = $"DELETE FROM {this.EntityTypeInfo.Name} WHERE id = {id}";

            await connection.ExecuteAsync(sql);
         }
      }

      public async Task<bool> ExistsAsync(int id)
      {
         var query = new Query(this.EntityTypeInfo.Name).Where("id", id);

         return (await this.GetSingleAsync(query)) == null;
      }

      public async Task<IEnumerable<ReturnType>> Function<ReturnType>(string functionName, params object[] parameters)
      {
         var parameterString = string.Join(",", parameters.Select(parameter =>
         {
            if (parameter.IsNumeric() || parameter is bool || parameter is null) return parameter?.ToString() ?? "NULL";
            else if (parameter is IEnumerable && !(parameter is string))
            {
               var sb = new StringBuilder("'{");
               foreach (var val in parameter as IEnumerable)
               {
                  sb.Append($"\"{val.ToString()}\",");
               }
               sb.Remove(sb.Length - 1, 1);
               sb.Append("}'");

               return sb.ToString();
            }
            else return $"'{parameter}'";
         }));

         using (var connection = this.CreateConnection())
         {
            var sql = $"SELECT * FROM {functionName}({parameterString})";
            var dbResult = await connection.QueryAsync<ReturnType>(sql);

            return dbResult;
         }
      }

      protected NpgsqlConnection CreateConnection() => new NpgsqlConnection(this.ConnectionString);

      protected QueryFactory QueryFactory(NpgsqlConnection connection) => new QueryFactory(connection, this.SqlCompiler);
   }
}