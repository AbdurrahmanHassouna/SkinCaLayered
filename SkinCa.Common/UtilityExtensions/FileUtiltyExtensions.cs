using Microsoft.AspNetCore.Http;

namespace SkinCa.Common.UtilityExtensions;

public static class FileUtiltyExtensions
{
    public static async Task<byte[]> ToBytesAsync(this IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}