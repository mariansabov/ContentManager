namespace ContentManager.Infrastructure.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int AccessTtlMinutes { get; set; } = 10;
        public string Key { get; set; } = null!;
    }
}
