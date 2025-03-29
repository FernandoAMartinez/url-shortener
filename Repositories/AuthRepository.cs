using Supabase.Gotrue;
using Supabase.Realtime;
using Supabase.Storage;
using UrlShortener.Contracts;
using UrlShortener.Helpers;

namespace UrlShortener.Repositories;
public class AuthRepository : IAuthRepository
{
    private Supabase.Interfaces.ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> _client;

    public AuthRepository(Supabase.Interfaces.ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> client)
    {
        _client = client;
    }
    public async Task<UserResponse> Register(RegisterRequest req)
    {
        var helper = new HashHelper();

        var model = new Models.User
        {
            Email = req.Email,
            Username = req.Username,
            Password = helper.HashPassword(req.Password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var response = await _client.From<Models.User>()
            .Insert(model);

        if (response.Models.Count.Equals(0))
            return null;

        var user = response.Models.FirstOrDefault();

        return new UserResponse(user.Id, user.Email, user.Username, user.CreatedAt, user.UpdatedAt);
    }

    public async Task<UserResponse> Login(LoginRequest req)
    {
        var helper = new HashHelper();

        var response = await _client.From<Models.User>()
            .Select("*")
            .Where(x => x.Email == req.Email)
            .Get();

        if (response.Models.Count.Equals(0))
            return null;

        var user = response.Models.FirstOrDefault();

        if (!helper.VerifyPassword(req.Password, user.Password))
            return null;

        return new UserResponse(user.Id, user.Email, user.Username, user.CreatedAt, user.UpdatedAt);
    }

}
