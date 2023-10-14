using System.IO;
using System.Threading.Tasks;
using System;

namespace HttpProgressBar
{
    public static class StreamExtensions
    {
        // Extension method to copy a stream with progress reporting
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, long? contentLength, IProgress<float> progress)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            ArgumentNullException.ThrowIfNull(bufferSize);
            if (contentLength.HasValue && contentLength.Value < 0) throw new ArgumentOutOfRangeException(nameof(contentLength));

            byte[] buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer)) > 0)
            {
                await destination.WriteAsync(buffer.AsMemory(0, bytesRead));
                totalBytesRead += bytesRead;
                if (contentLength.HasValue)
                {
                    // Calculate the progress percentage and report it
                    float percentage = (float)totalBytesRead / contentLength.Value * 100;
                    progress?.Report(percentage);
                }
            }
        }
    }
}
