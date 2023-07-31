using System.ComponentModel.DataAnnotations;

namespace xUnitSample.Controllers.Models;

public class UserRegistrationRequest
{
    /// <summary>
    /// 註冊帳號
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 信箱
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// 註冊密碼
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    /// <summary>
    /// 確認註冊密碼
    /// </summary>
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}