using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SessionMapper;

public static class CustomSessionExtensions
{
	/// <summary>
	/// Stores the object as a JSON string in the session under the given key.
	/// </summary>
	/// <param name="session">The session in which to store the object.</param>
	/// <param name="key">The key under which to store the object.</param>
	/// <param name="obj">The object to store in the session.</param>
	public static void SetAsJson<T>(this ISession session, string key, T obj)
	{
		var jsonObj = JsonConvert.SerializeObject(obj);
		session.Set(key, Encoding.UTF8.GetBytes(jsonObj));
	}
	
	/// <summary>
	/// Retrieves an object from the session that was previously stored as a JSON string.
	/// </summary>
	/// <typeparam name="T">The type of the object to retrieve.</typeparam>
	/// <param name="session">The session from which to retrieve the object.</param>
	/// <param name="key">The key under which the object was stored in the session.</param>
	/// <returns>
	/// The deserialized object of type <typeparamref name="T"/> if found in the session; otherwise, returns null.
	/// </returns>
	public static T? Get<T>(this ISession session, string key) where T : class
	{
		var isSuccess = session.TryGetValue(key, out var bytes);
		
		if (isSuccess && bytes != null)
		{
			var json = Encoding.UTF8.GetString(bytes);
			return JsonConvert.DeserializeObject<T>(json);
		}

		return null;
	}
}