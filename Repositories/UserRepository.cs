using Supabase.Gotrue;
using Supabase.Realtime;
using Supabase.Storage;
using UrlShortener.Contracts;

namespace UrlShortener.Repositories;

public class UserRepository : IUserRepository
{
    private Supabase.Interfaces.ISupabaseClient<Supabase.Gotrue.User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> _client;

    public UserRepository(Supabase.Interfaces.ISupabaseClient<Supabase.Gotrue.User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> client)
    {
        _client = client;
    }

    public async Task<UserResponse> ChangePassword(ChangePasswordRequest req)
    {
        var response = await _client.From<Models.User>()
            .Select("*")
            .Where(x => x.Id == req.Id && x.Password == req.OldPassword)
            .Get();

        if (response.Models.Count.Equals(0))
            return null;

        var user = response.Models.FirstOrDefault();

        user.Password = req.NewPassword;

        var update = await _client.From<Models.User>()
            .Where(x => x.Id == req.Id)
            .Set(x => x.Password, req.NewPassword)
            .Update();

        if (update.Models.Count.Equals(0))
            return null;

        user = update.Models.FirstOrDefault();

        return new UserResponse(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt);
    }

    public async Task DeleteUser(long id)
    {
        await _client
          .From<Models.User>()
          .Where(x => x.Id == id)
          .Delete();
    }

    public async Task<UserResponse> GetUserById(long id)
    {
        var response = await _client.From<Models.User>()
            .Select("*")
            .Where(x => x.Id == id)
            .Get();

        if (response.Models.Count.Equals(0))
            return null;

        var user = response.Models.FirstOrDefault();

        return new UserResponse(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt);
    }

    public async Task<IEnumerable<UserResponse>> GetUsers()
    {
        var response = await _client.From<Models.User>()
            .Select("*")
            .Get();

        if (response.Models.Count.Equals(0))
            return null;

        var users = new List<UserResponse>();

        foreach (var user in response.Models)
        {
            users.Add(new UserResponse(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt));
        }

        return users;
    }

    public async Task UpdateUser(UserUpdateRequest req)
    {
        var model = await _client.From<Models.User>()
            .Where(x => x.Id == req.Id)
            .Single();

        if (!string.IsNullOrEmpty(req.Username))
            model.Username = req.Username;

        if (!string.IsNullOrEmpty(req.Email))
            model.Email = req.Email;

        model.UpdatedAt = DateTime.Now;

        await model.Update<Models.User>();
    }
}