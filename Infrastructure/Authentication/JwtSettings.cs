// multiple editing ctrl+F,  then alt+enter
namespace Infrastructure.Authentication
{
    public class JwtSettings
    {
        public static string SectionName { get; internal set; } = "JwtSettings";
        public string Secret { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpiryInMinutes { get; init; }
    }
}