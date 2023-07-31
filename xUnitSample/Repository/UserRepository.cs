using xUnitSample.Repository.Models;

namespace xUnitSample.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<Users> _userTable;

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
                    PasswordHash = "AQAAAAEAACcQAAAAEDalRx56AeFrm2mEV30jKeKYgQIGCFPH/1nsiLl7FNx6Xr/ksBDk7+WtzSqqlUWmag==",
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
                    PasswordHash = "AQAAAAEAACcQAAAAEPPoqypM4VETr9G6yZrf67DsIMOUb+8P+E4UIz1eAJzsIAJv2CDygIImq35qH8J0pA==",
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
                    PasswordHash = "AQAAAAEAACcQAAAAEJryJGFY+aCPjrZZxoehT6Z1vj8XcRR4fPEG6VRGXJC+7El/nTaPJw/EvalatHsKVQ==",
                    CreatedAt = new DateTime(2023, 1, 1),
                    UpdatedAt = new DateTime(2023, 1, 1),
                    Enable = true
                }
            };
        }

        /// <summary>
        /// 依帳號取得資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Users?> GetByIdAsync(string id)
        {
            return _userTable.FirstOrDefault(x => x.Id == id && x.Enable);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(Users user)
        {
            var beforeCount = _userTable.Count;
            user.Seq = _userTable.MaxBy(x => x.Seq).Seq + 1;
            _userTable.Add(user);
            return _userTable.Count > beforeCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Users user)
        {
            var index = _userTable.FindIndex(x => x.Seq == user.Seq);
            _userTable[index] = user;
            return true;
        }
    }
}