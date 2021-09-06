using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public class DbEntityTypeInfo<T>
{
   public string Name { get; }
   public string[] Props { get; }

   private readonly static Func<T, string, object> getPropValue;

   static DbEntityTypeInfo()
   {
      getPropValue = InitializeGetValueAction();
   }

   public DbEntityTypeInfo(string entityName = null, IEnumerable<string> entityNames = null)
   {
      this.Name = entityName ?? this.PascalToKebab(nameof(T));
      var propertyNames = typeof(T).GetProperties().Select(x => x.Name);
      this.Props = entityNames?.ToArray() ?? propertyNames.Select(this.PascalToKebab).ToArray();
   }

   public Dictionary<string, string> GetValuePairs(T entity)
   {
      var retVal = new Dictionary<string, string>();

      foreach (var prop in this.Props)
      {
         retVal.Add(prop, getPropValue(entity, this.KebabToPascal(prop)).ToString());
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
            Expression.Constant(new ArgumentException($"The provided string matches no property of {nameof(T)}")
            )
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

   private string PascalToKebab(string pascal) => Regex.Replace(
            pascal,
            "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
            "-$1",
            RegexOptions.Compiled)
            .Trim()
            .ToLower();

   private string KebabToPascal(string kebab)
   {
      kebab = kebab.ToLower().Replace("_", " ");
      System.Globalization.TextInfo info = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
      var pascal = info.ToTitleCase(kebab).Replace(" ", string.Empty);

      return pascal;
   }
}