namespace xUnitSample.Service.Models;

public class CreateUserDto
{
    /// <summary>
    /// 帳號
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; }
}