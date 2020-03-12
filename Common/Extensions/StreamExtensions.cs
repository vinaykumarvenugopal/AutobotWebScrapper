using System.IO;
using System.Threading.Tasks;

namespace AutobotWebScrapper.Common.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<string> StreamToStringAsync(this Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }
    }
}
