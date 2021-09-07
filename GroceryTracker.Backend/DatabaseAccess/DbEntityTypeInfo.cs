using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GroceryTracker.Backend.ExtensionMethods;

namespace GroceryTracker.Backend.DatabaseAccess
{
   public interface IDbEntityTypeInfo<T>
   {
      /// <summary>
      /// Name of the entity type in kebab_case (analogous to table name in DB)
      /// </summary>
      string Name { get; }

      /// <summary>
      /// Dictionary: Keys are Property Name of Model class in PascalCase
      /// values are prop names of the entity type in kebab_case (analogous to DB)
      /// </summary>
      Dictionary<string, string> Props { get; }

      /// <summary>
      /// Get a dictionary with all values of the entity type's fields, indexed with their names
      /// </summary>
      /// <param name="entity">The entity whose values you want to retrieve</param>
      Dictionary<string, string> GetValuePairs(T entity);
   }

   public class DbEntityTypeInfo<T> : IDbEntityTypeInfo<T>
   {
      public string Name { get; }

      public Dictionary<string, string> Props { get; }

      private readonly static Func<T, string, object> getPropValue;

      static DbEntityTypeInfo()
      {
         getPropValue = InitializeGetValueAction();
      }

      public DbEntityTypeInfo(string entityName = null, IEnumerable<string> entityProps = null)
      {
         this.Name = entityName ?? nameof(T).PascalToKebab();
         var propertyNames = typeof(T).GetProperties().Select(x => x.Name);
         this.Props = new Dictionary<string, string>(
            entityProps?.Select(x => new KeyValuePair<string, string>(x, x.PascalToKebab())));
      }

      public Dictionary<string, string> GetValuePairs(T entity)
      {
         var retVal = new Dictionary<string, string>();

         foreach (var prop in this.Props)
         {
            var name = prop.Value;
            var value = getPropValue(entity, prop.Key);
            retVal.Add(name, value.ToString());
         }

         return retVal;
      }

      private static Func<T, string, object> InitializeGetValueAction()
      {
         // Define the parameters of the Func that will be returned so they can be used as Expressions 
         var instanceParameterExpression = Expression.Parameter(typeof(T), "instance");
         var stringParameterExpression = Expression.Parameter(typeof(string), "propertyName");

         // Define a List that can be filled with new Expressions and in the end be used to create an Expression block 
         // that will constitute the body of the returned Func
         var finalExpressions = new List<Expression>();

         // This label is needed as a point to jump to when breaking out of the Expression block that will be defined in two statements.
         // Return and break statements in Expressions work like the deprecated GoTo statement, therefore a continuation point must be specified
         // The type argument must be the same as the return type of the function.
         var label = Expression.Label(typeof(object), "break");

         // Create an Expression for each property of the given Type
         foreach (var property in typeof(T).GetProperties().Select(x => x.Name))
         {
            // Constant Expression that holds the name of the currently processed property  
            var propertyNameExpression = Expression.Constant(property);

            // Expression that holds meta information about the property that is currently processed
            var propertyExpression = Expression.Property(instanceParameterExpression, property);

            var parameterEqualsPropertyNameExpression =
               Expression.Equal(                            // Generate a comparison for equality, comparing ..
               propertyNameExpression,                      // .. the name of the property with ..
                  stringParameterExpression                 // .. the value of the string parameter (name of the property that you want the value of).
               );                                           // => "CurrentProperty" == "WantedProperty"

            finalExpressions.Add(                           // Add following Expression to the list of Expressions to be executed:
               Expression.IfThen(                           // Generate an if-statement. If ..
                  parameterEqualsPropertyNameExpression,    // .. the equality comparison from above evaluates to true .. 
                  Expression.Return(                        // .. return ..
                     label,                                 // .. to this label (similar to a GoTo statement).
                     Expression.Convert(                    // Return value is the result of a type conversion ..
                        propertyExpression,                 // .. that converts the value of the current property ..
                        typeof(object)                      // .. to Type object (return type of the function)
                     )
                  )
               )                                            // => if ("CurrentProperty" == "WantedProperty") return instance.CurrentProperty;
            );
         }

         // After all properties have been checked, add an Expression that throws an Exception, 
         // so it throws when the provided name doesn't match any of the type's properties
         finalExpressions.Add(
            Expression.Throw(
               Expression.Constant(new ArgumentException($"The provided string matches no property of {nameof(T)}"))
            )
         );

         // This Expression is needed because the last Expression of a block for a Func must be a non-void value that can always be returned
         // This will never be returned, because in any case the Exception before will be thrown.
         finalExpressions.Add(Expression.Label(label, Expression.Constant(null)));

         // Define a BlockExpression that contains all Expressions that have been added to finalExpressions in the same Contact.
         var functionBody = Expression.Block(finalExpressions);

         // Make a single Expression tree that contains information for executing the built-up function body 
         // with the provided parameter expressions and compile the result into a simple Func<T, string, object> and return it.
         return Expression.Lambda<Func<T, string, object>>(functionBody, instanceParameterExpression, stringParameterExpression).Compile();
      }
   }
}