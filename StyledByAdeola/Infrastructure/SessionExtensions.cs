//The session state feature in ASP.NET Core stores only int, string, and byte[] values.
//Since I want to store a Cart object, I need to define extension methods to the ISession interface,  
//which provides access to the session state data to serialize Cart objects into JSON and convert them back


using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace StyledByAdeola.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetJson<T>(this ISession session, string key)
        {
            // get JSON data from sessions
            var sessionData = session.GetString(key);

            // take jSON formatted session data and convert it to T object e.g cart obj
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
