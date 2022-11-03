using System.Text;

namespace AlinSpace.Development.Cli
{
    /// <summary>
    /// Extensions for <see cref="StreamReader"/>.
    /// </summary>
    public static class StreamReaderExtensions
    {
        /// <summary>
        /// To string asynchronously.
        /// </summary>
        /// <param name="streamReader">Stream reader.</param>
        /// <param name="encoding">Encoding.</param>
        /// <returns>String.</returns>
        public static async Task<string> ToStringAsync(this StreamReader streamReader, Encoding encoding = null)
        {
            using var memoryStream = new MemoryStream();

            await streamReader.BaseStream.CopyToAsync(memoryStream);

            var bytes = memoryStream.ToArray();

            var text = (encoding ?? Encoding.UTF8).GetString(bytes);

            return text;
        }
    }
}
