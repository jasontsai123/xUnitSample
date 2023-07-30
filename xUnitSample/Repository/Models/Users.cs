namespace xUnitSample.Repository.Models;

public class Users
{
    /// <summary>
    /// 帳號
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 密碼(加密過)
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// 加密用鹽
    /// </summary>
    public string EncryptedSalt { get; set; }

    /// <summary>
    /// 新增日期
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// 修改日期
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// 啟停用
    /// </summary>
    public bool Enable { get; set; }
}