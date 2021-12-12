namespace TruffleMarketApi.Services.Authentication
{
    public class JwtAuthenticationConfig
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}
