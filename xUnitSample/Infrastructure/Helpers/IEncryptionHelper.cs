namespace xUnitSample.Infrastructure.Helpers;

public interface IEncryptionHelper
{
    /// <summary>
    /// 加密字串.
    /// </summary>
    /// <param name="text">要加密的字串</param>
    /// <param name="key">加密的密碼</param>
    /// <returns></returns>
    string EncryptText(string text, string key);

    /// <summary>
    /// 解密字串.
    /// </summary>
    /// <param name="text">要解密的字串.</param>
    /// <param name="key">解密的密碼.</param>
    /// <returns></returns>
    string DecryptText(string text, string key);

    /// <summary>
    /// 產生雜湊簽章.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="salt">The salt.</param>
    /// <returns></returns>
    string HashText(string text, string salt);
}