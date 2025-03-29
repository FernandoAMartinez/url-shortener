using Microsoft.VisualBasic;
using UrlShortener.Contracts;

namespace UrlShortener.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserResponse>> GetUsers();

    Task<UserResponse> GetUserById(long id);

    Task<UserResponse> ChangePassword(ChangePasswordRequest req);

    Task UpdateUser(UserUpdateRequest req);
    Task DeleteUser(long id);
}
