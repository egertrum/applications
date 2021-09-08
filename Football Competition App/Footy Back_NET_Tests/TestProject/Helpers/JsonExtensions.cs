using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestProject.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions =
            new JsonSerializerOptions(JsonSerializerDefaults.Web);
        
        public static async Task<TValue?> DeserializeWithWebDefaults<TValue>(HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            var obj = System.Text.Json.JsonSerializer.Deserialize<TValue>(json, JsonSerializerOptions);
            return obj;
        }
        
        public static string? SerializeWithWebDefaults<TValue>(TValue obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }

    }

}