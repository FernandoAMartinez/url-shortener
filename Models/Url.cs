using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UrlShortener.Models;

[Table("Urls")]
public class Url : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }
    [Column("address")]
    public string UrlAddress { get; set; }
    [Column("short_code")]
    public string ShortCode { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    [Column("access_count")]
    public long AccessCount{ get; set; }
}