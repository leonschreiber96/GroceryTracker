using System;

public struct SessionToken
{
   public string Token { get; set; }

   public DateTime ExpirationDate { get; set; }
}