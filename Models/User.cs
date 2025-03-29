using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UrlShortener.Models;

[Table("Users")]
public class User : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("encrypted_password")]
    public string Password { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}