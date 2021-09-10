using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GroceryTracker.Backend.ExtensionMethods
{
   public static class ObjectExtensions
   {
      public static bool IsNumeric(this object value)
      {
         return value is sbyte
            || value is byte
            || value is short
            || value is ushort
            || value is int
            || value is uint
            || value is long
            || value is ulong
            || value is float
            || value is double
            || value is decimal;
      }
   }
}