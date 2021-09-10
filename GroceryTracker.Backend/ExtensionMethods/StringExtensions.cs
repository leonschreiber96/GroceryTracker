using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GroceryTracker.Backend.ExtensionMethods
{
   public static class StringExtensions
   {
      public static string Sha256(this string instance)
      {
         // Create a SHA256   
         using (var sha256Hash = SHA256.Create())
         {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(instance));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
               builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
         }
      }

      public static string PascalToKebab(this string pascal)
      {
         var regex = "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])";
         var kebab = Regex.Replace(pascal, regex, "_$1", RegexOptions.Compiled).Trim().ToLower();

         return kebab;
      }

      public static string KebabToPascal(this string kebab)
      {
         kebab = kebab.ToLower().Replace("_", " ");
         System.Globalization.TextInfo info = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
         var pascal = info.ToTitleCase(kebab).Replace(" ", string.Empty);

         return pascal;
      }
   }
}