using System.Text;

namespace AlinSpace.Development.Cli
{
    public static class StreamReaderExtensions
    {
        public static async Task<string> ReadToStringAsync(this StreamReader streamReader, Encoding encoding = null)
        {
            using var memoryStream = new MemoryStream();

            await streamReader.BaseStream.CopyToAsync(memoryStream);

            var bytes = memoryStream.ToArray();

            var text = (encoding ?? Encoding.UTF8).GetString(bytes);

            return text;
        }
    }
}
