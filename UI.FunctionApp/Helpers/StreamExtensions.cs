using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UI.FunctionApp.Helpers
{
    internal static class StreamExtensions
    {
        internal static async Task<T> DeserializeAsync<T>(
           this Stream stream)
        {
            var sr =
                new StreamReader(stream);
            var json =
                await sr.ReadToEndAsync();
            var data =
                JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }
}
