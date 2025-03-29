namespace UrlShortener.Contracts;
public record ChangePasswordRequest(long Id, string OldPassword, string NewPassword);