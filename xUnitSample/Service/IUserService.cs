using xUnitSample.Service.Models;

namespace xUnitSample.Service;

public interface IUserService
{
    /// <summary>
    /// 個人資訊
    /// </summary>
    /// <param name="id">帳號</param>
    /// <returns></returns>
    Task<UserInfoDto> GetAsync(string id);

    /// <summary>
    /// 驗證帳號
    /// </summary>
    /// <param name="id">帳號</param>
    /// <param name="password">密碼</param>
    /// <returns></returns>
    Task<AuthenticateDto> AuthenticateAsync(string id, string password);

    /// <summary>
    /// 新增使用者
    /// </summary>
    /// <param name="createUser">The users dto.</param>
    /// <returns></returns>
    Task<bool> CreateAsync(CreateUserDto createUser);
}