namespace MediaShop.Configuration;

public class AuthSettings
{
    public required string Secret { get; set; }
    public required string Audience { get; set; }
    public required string Issuer { get; set; }
}
