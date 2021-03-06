using System.IO;
using System.Threading.Tasks;
using Cosmos.Conversions;

namespace Cosmos.Serialization.Binary
{
    /// <summary>
    /// Binary helper
    /// </summary>
    public static partial class BinaryHelper
    {
        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async Task<byte[]> SerializeAsync(object obj)
        {
#if NETFRAMEWORK || NETSTANDARD2_0
            using var stream = await PackAsync(obj);
#else
            await using var stream = await PackAsync(obj);
#endif
            return await stream.CastToBytesAsync();
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> DeserializeAsync<T>(byte[] bytes)
        {
            return (T) await DeserializeAsync(bytes);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static async Task<object> DeserializeAsync(byte[] bytes)
        {
            if (bytes is null || bytes.Length is 0)
                return default;
#if NETFRAMEWORK || NETSTANDARD2_0
            using var ms = new MemoryStream(bytes);
#else
            await using var ms = new MemoryStream(bytes);
#endif
            return await UnpackAsync(ms);
        }
    }
}