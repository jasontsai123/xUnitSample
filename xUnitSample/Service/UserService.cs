using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using xUnitSample.Infrastructure.Helpers;
using xUnitSample.Repository;
using xUnitSample.Repository.Models;
using xUnitSample.Service.Models;

namespace xUnitSample.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly EncryptionHelper _encryptionHelper;
        public UserService(UserRepository userRepository, EncryptionHelper encryptionHelper)
        {
            _userRepository = userRepository;
            _encryptionHelper = encryptionHelper;
        }

        /// <summary>
        /// 個人資訊
        /// </summary>
        /// <param name="id">帳號</param>
        /// <returns></returns>
        public async Task<UserInformationDto> GetAsync(string id)
        {
            var hpUsers = await this._userRepository.GetByAccountAsync(id);
            if (HPUsersExists(hpUsers) == false)
            {
                return new UserInformationDto();
            }
            var hpUser = hpUsers.FirstOrDefault();

            var userInformationDto = new UserInformationDto()
            {
                GroupId = hpUser.Group_id,
                Name = hpUser.Name,
                Enable = hpUser.Enable,
                Account = hpUser.Account
            };

            return userInformationDto;
        }

        /// <summary>
        /// 驗證帳號
        /// </summary>
        /// <param name="id">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public async Task<AuthenticateDto> AuthenticateAsync(string id, string password)
        {
            var loginFailDto = new AuthenticateDto()
            {
                UserId = "",
                IsAuthenticated = false
            };

            var hpUsers = await this._userRepository.GetById(id);
            if (HPUsersExists(hpUsers) == false)
            {
                return loginFailDto;
            }

            // 驗證密碼
            var result =  new PasswordHasher<string>().VerifyHashedPassword(id, user.PasswordHash, "User1Password");
            if (result == PasswordVerificationResult.Success)

            var hpUser = hpUsers.FirstOrDefault();
            var passwordHash = this._EncryptionHelper.HashText(password, hpUser.EncryptedSalt);
            if (hpUser.PasswordHash != passwordHash || hpUser.Enable == false)
            {
                return loginFailDto;
            }

            return new AuthenticateDto()
            {
                IsLogin = true,
                Account = hpUser.Account
            };
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="usersParameterDto">The users dto.</param>
        /// <returns></returns>
        public async Task<int> CreateAsync(HPUsersDto param)
        {
            // 確認帳號有沒有存在
            var hpUsers = await this._UsersRepository.GetByAccountAsync(param.Account);
            if (HPUsersExists(hpUsers) == true)
            {
                return 0;
            }

            // 儲存密碼雜湊
            var hasher = new PasswordHasher<string>();
            user.PasswordHash = hasher.HashPassword(id, password);

            // 驗證密碼
            var result =  new PasswordHasher<string>().VerifyHashedPassword(user, user.PasswordHash, "User1Password");
            if (result == PasswordVerificationResult.Success)
            {
                Console.WriteLine("密碼驗證成功");
            }
            else
            {
                Console.WriteLine("密碼驗證失敗");
            }
            var salt = CreateSalt(param.Account + param.Password);
            var hpUser = new HPUsers()
            {
                Group_id = param.GroupId,
                Name = param.Name,
                PasswordHash = this._EncryptionHelper.HashText(param.Password, salt),
                EncryptedSalt = salt,
                CreateDateTime = DateTime.Now,
                Enable = true,
                Account = param.Account
            };
            return await this._UsersRepository.InsertAsync(hpUser);
        }

        /// <summary>
        /// 加鹽
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        private string CreateSalt(string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(salt);
            return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(bytes));
        }
    }
}
