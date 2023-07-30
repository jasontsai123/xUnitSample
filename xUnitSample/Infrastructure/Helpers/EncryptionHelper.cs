using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace xUnitSample.Infrastructure.Helpers;

public class EncryptionHelper : IEncryptionHelper
{
    // The salt bytes must be at least 8 bytes.
    private static readonly byte[] SaltBytes = { 2, 7, 0, 5, 1, 3, 8, 0 };

    /// <summary>
    /// 加密字串.
    /// </summary>
    /// <param name="text">要加密的字串</param>
    /// <param name="key">加密的密碼</param>
    /// <returns></returns>
    public string EncryptText(string text, string key)
    {
        var sourceBytes = Encoding.UTF8.GetBytes(text);
        var keyBytes = Encoding.UTF8.GetBytes(key);

        keyBytes = SHA256.Create().ComputeHash(keyBytes);

        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 256;
        aes.BlockSize = 128;

        var rfcBytes = new Rfc2898DeriveBytes(keyBytes, SaltBytes, 1000);
        aes.Key = rfcBytes.GetBytes(aes.KeySize / 8);
        aes.IV = rfcBytes.GetBytes(aes.BlockSize / 8);

        var transform = aes.CreateEncryptor();

        var result = Convert.ToBase64String(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));

        return result;
    }

    /// <summary>
    /// 解密字串.
    /// </summary>
    /// <param name="text">要解密的字串.</param>
    /// <param name="key">解密的密碼.</param>
    /// <returns></returns>
    public string DecryptText(string text, string key)
    {
        var sourceBytes = Convert.FromBase64String(text);
        var keyBytes = Encoding.UTF8.GetBytes(key);

        keyBytes = SHA256.Create().ComputeHash(keyBytes);

        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 256;
        aes.BlockSize = 128;

        var rfcBytes = new Rfc2898DeriveBytes(keyBytes, SaltBytes, 1000);
        aes.Key = rfcBytes.GetBytes(aes.KeySize / 8);
        aes.IV = rfcBytes.GetBytes(aes.BlockSize / 8);

        var transform = aes.CreateDecryptor();

        var result = Encoding.UTF8.GetString(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));

        return result;
    }

    /// <summary>
    /// 產生雜湊簽章.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="salt">The salt.</param>
    /// <returns></returns>
    public string HashText(string text, string salt)
    {
        var scope = 10 + 26 + 26; // 0-9 A-Z a-z 

        try
        {
            var sourceBytes = Encoding.UTF8.GetBytes(text + salt);
            var hashBytes = SHA256.Create().ComputeHash(sourceBytes);
            var encodeString = BitConverter.ToString(hashBytes);

            var tokens = encodeString.Split('-');
            var result = "";

            foreach (var hex in tokens)
            {
                var num = int.Parse(hex, NumberStyles.HexNumber);
                var r = num % scope;
                if (r >= 0 && r < 10)
                {
                    result += Convert.ToChar(r + 48);
                }
                else if (r >= 10 && r < 36)
                {
                    r -= 10;
                    result += Convert.ToChar(r + 65);
                }
                else
                {
                    r -= 36;
                    result += Convert.ToChar(r + 97);
                }
            }

            return result;
        }
        catch
        {
            return "";
        }
    }
}