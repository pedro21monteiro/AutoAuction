using UserService.Models;

namespace UserService.Services
{
    public interface ITokenService
    {
        AuthResponseDto GenerateToken(User user);
    }
}
