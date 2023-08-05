using System.Security.Cryptography;
using xUnitSample.Infrastructure.Helpers;
using xUnitSample.Repository.Models;

namespace xUnitSample.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<Users> _userTable;
        private readonly List<UserPasswords> _userPasswordsTable;

        public UserRepository()
        {
            _userTable = new List<Users>
            {
                new Users
                {
                    Seq = 1,
                    Id = "user1",
                    Name = "Josh",
                    Email = "Josh23423@gmail.com",
                    CreatedAt = new DateTime(2022, 10, 3),
                    UpdatedAt = new DateTime(2022, 10, 3),
                    Enable = true
                },
                new Users
                {
                    Seq = 2,
                    Id = "user2",
                    Name = "David",
                    Email = "123David@gmail.com",
                    CreatedAt = new DateTime(2022, 11, 4),
                    UpdatedAt = new DateTime(2022, 11, 4),
                    Enable = true
                },
                new Users
                {
                    Seq = 3,
                    Id = "user3",
                    Name = "Emma",
                    Email = "Emma32@yahoo.com",
                    CreatedAt = new DateTime(2023, 1, 1),
                    UpdatedAt = new DateTime(2023, 1, 1),
                    Enable = true
                }
            };

            _userPasswordsTable = new List<UserPasswords>
            {
                new UserPasswords
                {
                    Id = 1,
                    UserId = "user1",
                    PasswordHash = "AQAAAAEAACcQAAAAEDalRx56AeFrm2mEV30jKeKYgQIGCFPH/1nsiLl7FNx6Xr/ksBDk7+WtzSqqlUWmag==",
                    CreatedAt = new DateTime(2022, 10, 3),
                    RowVersion = RandomNumberGenerator.GetBytes(8)
                },
                new UserPasswords
                {
                    Id = 2,
                    UserId = "user1",
                    PasswordHash = "AQAAAAEAACcQAAAAEDalRx56AeFrm2mEV30jKeKYgQIGCFPH/1nsiLl7FNx6Xr/ksBDk7+WtzSqqlUWmag==",
                    CreatedAt = new DateTime(2022, 11, 9),
                    RowVersion = RandomNumberGenerator.GetBytes(8)
                },
                new UserPasswords
                {
                    Id = 3,
                    UserId = "user2",
                    PasswordHash = "AQAAAAEAACcQAAAAEDalRx56AeFrm2mEV30jKeKYgQIGCFPH/1nsiLl7FNx6Xr/ksBDk7+WtzSqqlUWmag==",
                    CreatedAt = new DateTime(2022, 11, 4),
                    RowVersion = RandomNumberGenerator.GetBytes(8)
                },
                new UserPasswords
                {
                    Id = 4,
                    UserId = "user3",
                    PasswordHash = "AQAAAAEAACcQAAAAEDalRx56AeFrm2mEV30jKeKYgQIGCFPH/1nsiLl7FNx6Xr/ksBDk7+WtzSqqlUWmag==",
                    CreatedAt = new DateTime(2023, 1, 1),
                    RowVersion = RandomNumberGenerator.GetBytes(8)
                }
            };
        }

        /// <summary>
        /// 依帳號取得資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDataModel?> GetByIdAsync(string id)
        {
            var result = _userTable.GroupJoin(_userPasswordsTable, u => u.Id, p => p.UserId, (user, pwd) => new { user, pwd })
                .GroupBy(g => g.user.Id)
                .SelectMany(g => g.DefaultIfEmpty().Take(1), (g, p) =>
                {
                    var user = g.FirstOrDefault().user;
                    var pwd = g.FirstOrDefault().pwd.OrderByDescending(o => o.CreatedAt).FirstOrDefault();
                    return new UserDataModel()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PasswordHash = pwd.PasswordHash,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = pwd.CreatedAt,
                        Enable = user.Enable
                    };
                }).FirstOrDefault(x => x.Id == id && x.Enable);
            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(UserDataModel data)
        {
            var beforeCount = _userTable.Count;
            var pwdBeforeCount = _userPasswordsTable.Count;
            var user = new Users
            {
                Seq = _userTable.MaxBy(x => x.Seq).Seq + 1,
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                Enable = data.Enable
            };
            _userTable.Add(user);
            var pwd = new UserPasswords
            {
                Id = _userPasswordsTable.MaxBy(x => x.Id).Id + 1,
                UserId = data.Id,
                PasswordHash = data.PasswordHash,
                CreatedAt = data.CreatedAt,
                RowVersion = RandomNumberGenerator.GetBytes(8)
            };
            _userPasswordsTable.Add(pwd);
            return _userTable.Count > beforeCount && _userPasswordsTable.Count > pwdBeforeCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(UserDataModel data)
        {
            var index = _userTable.FindIndex(x => x.Id == data.Id);
            var user = _userTable[index];
            _userTable[index] = new Users
            {
                Seq = user.Seq,
                Id = user.Id,
                Name = data.Name,
                Email = data.Email,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                Enable = data.Enable
            };
            return ObjectHelper.Compare(user, _userTable[index]) == false;
        }
    }
}