using UserService.Dao;
using UserService.Models;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDao _userDao;
        private readonly ITokenService _tokenService;

        public UserService(IUserDao userDao, ITokenService tokenService)
        {
            _userDao = userDao ?? throw new ArgumentNullException(nameof(userDao));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            if (await _userDao.ExistsAsync(dto.Email, dto.Username))
            {
                return null;
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userDao.AddAsync(user);

            return _tokenService.GenerateToken(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userDao.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null;
            }

            user.LastLogin = DateTime.UtcNow;
            await _userDao.UpdateAsync(user);

            return _tokenService.GenerateToken(user);
        }
    }
}
