using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using xUnitSample.Repository;
using xUnitSample.Repository.Models;
using xUnitSample.Service.Models;

namespace xUnitSample.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 個人資訊
        /// </summary>
        /// <param name="id">帳號</param>
        /// <returns></returns>
        public async Task<UserInfoDto> GetAsync(string id)
        {
            var user = await this._userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return new UserInfoDto();
            }

            return new UserInfoDto
            {
                Id = user.Id,
                Name = user.Name
            };
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

            var user = await this._userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return loginFailDto;
            }

            // 驗證密碼
            var verifyHashedPassword =  new PasswordHasher<string>().VerifyHashedPassword(id, user.PasswordHash, password);
            if (verifyHashedPassword != PasswordVerificationResult.Success || user.Enable == false)
            {
                return loginFailDto;
            }

            return new AuthenticateDto()
            {
                UserId = user.Id,
                IsAuthenticated = true
            };
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="createUser">The users dto.</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(CreateUserDto createUser)
        {
            // 確認帳號有沒有存在
            var user = await this._userRepository.GetByIdAsync(createUser.Id);
            if (user is null)
            {
                return false;
            }

            // 儲存密碼雜湊
            var hasher = new PasswordHasher<string>();
            var passwordHash = hasher.HashPassword(createUser.Id, createUser.Password);
            var addUserDto = new Users
            {
                Id = createUser.Id,
                Name = createUser.Name,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Enable = true
            };
            return await this._userRepository.InsertAsync(addUserDto);
        }
    }
}
