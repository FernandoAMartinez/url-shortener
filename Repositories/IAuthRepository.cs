using UrlShortener.Contracts;

namespace UrlShortener.Repositories;

public interface IAuthRepository
{
    Task<UserResponse> Login(LoginRequest request);

    Task<UserResponse> Register(RegisterRequest request);
}
