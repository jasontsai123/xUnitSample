using xUnitSample.Repository.Models;

namespace xUnitSample.Repository;

public interface IUserRepository
{
    /// <summary>
    /// 依帳號取得資料
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserDataModel?> GetByIdAsync(string id);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<bool> InsertAsync(UserDataModel data);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(UserDataModel data);
}