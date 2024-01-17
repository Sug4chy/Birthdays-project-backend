namespace Domain.Configs;

public class JwtConfigurationOptions
{
    public const string Position = "JwtConfiguration";
    
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SymmetricSecurityKey { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public int AccessTokenExpiresTimeInMinutes { get; set; }
    public int RefreshTokenExpiresTimeInDays { get; set; }
}