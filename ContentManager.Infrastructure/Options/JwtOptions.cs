namespace ContentManager.Infrastructure.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int AccessTtlMinutes { get; set; } = 10;
        public string Key { get; set; } = default!;
    }
}
