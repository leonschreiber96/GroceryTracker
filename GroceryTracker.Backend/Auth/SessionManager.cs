using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GroceryTracker.Backend.Auth
{
   public interface ISessionManager
   {
      TimeSpan SessionLength { get; }

      /// <summary>
      /// Creates a new session with the default expiry length for the provided user.
      /// </summary>
      /// <returns>Returns the session token for the newly created session.</returns>
      SessionToken CreateSession(int userId);

      /// <summary>
      /// Renews the session with the provided token
      /// </summary>
      /// /// <returns>Returns the session token for the renewed session.</returns>
      SessionToken RenewSession(string sessionToken);

      /// <summary>
      /// End the session with the provided token (i.e. deleting the token from storage).
      /// </summary>
      void EndSession(string sessionToken);

      /// <summary>
      /// Gets the session token for the provided user's current session (if one exists)
      /// </summary>
      /// /// <returns>Returns a session token if one for the user exists. Else returns null.</returns>
      SessionToken GetSession(int userId);

      /// <summary>
      /// Gets the user id for a provided session token
      /// </summary>
      /// /// <returns>Returns a user id if the provided sesssion token is legit. Else returns null.</returns>
      int GetUserForSession(string sessionToken);
   }

   public class SessionManager : ISessionManager
   {
      private Dictionary<int, SessionToken> sessions = new Dictionary<int, SessionToken>();

      public TimeSpan SessionLength { get; }

      public SessionManager(TimeSpan sessionLength)
      {
         this.SessionLength = sessionLength;
      }

      public SessionToken CreateSession(int userId, string sessionToken = null)
      {
         if (this.sessions.ContainsKey(userId))
         {
            this.EndSession(this.sessions[userId].Token);
         }

         using (var sha256 = SHA256.Create())
         {
            var token = "";

            do { token = this.createToken(); }
            while (this.sessionExists(token));

            var newSession = new SessionToken()
            {
               Token = token,
               ExpirationDate = DateTime.Now + this.SessionLength
            };

            this.sessions.Add(userId, newSession);

            return newSession;
         }
      }

      public void EndSession(string sessionToken)
      {
         if (sessionExists(sessionToken))
         {
            var key = this.GetUserForSession(sessionToken);
            this.sessions.Remove(key);
         }
      }

      public SessionToken GetSession(int userId) => this.sessions[userId];

      public SessionToken RenewSession(string sessionToken)
      {
         var userId = this.GetUserForSession(sessionToken);
         this.EndSession(sessionToken);

         var newSession = new SessionToken()
         {
            Token = sessionToken,
            ExpirationDate = DateTime.Now + this.SessionLength
         };

         this.sessions.Add(userId, newSession);

         return newSession;
      }

      public int GetUserForSession(string sessionToken) => this.sessions.First(x => x.Value.Token == sessionToken).Key;

      private bool sessionExists(string sessionToken) => this.sessions.Any(x => x.Value.Token == sessionToken);

      private string createToken()
      {
         using (var sha256 = SHA256.Create())
         {
            var guid = Guid.NewGuid();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(guid.ToString()));
            var token = Encoding.Default.GetString(hashBytes);

            return token;
         }
      }

      public SessionToken CreateSession(int userId)
      {
         throw new NotImplementedException();
      }
   }
}