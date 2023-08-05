namespace xUnitSample.Repository.Models;

public class UserPasswords
{
    /// <summary>
    /// 唯一識別欄位
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 密碼(加密過)
    /// </summary>
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// 密碼創建日期
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// RowVersion
    /// </summary>
    public byte[] RowVersion { get; set; }
}