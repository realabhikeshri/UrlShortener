using System.Security.Cryptography;
using System.Text;

namespace UrlShortener.Domain.Services;

public static class ShortCodeGenerator
{
    private const string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Deterministic hash-based generator
    public static string Generate(string input, int length = 8)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        var value = BitConverter.ToUInt64(hashBytes, 0);
        var chars = new char[length];

        for (int i = 0; i < length; i++)
        {
            chars[i] = Base62Chars[(int)(value % 62)];
            value /= 62;
        }

        return new string(chars);
    }
}
