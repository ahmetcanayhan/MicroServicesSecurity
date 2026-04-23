namespace AuthServer.Models
{
    public record ClientConfigDto
    {
        public string ClientId { get; init; } = null!;
        public string ClientSecret { get; init; } = null!;
        public string GrantType { get; init; } = null!;
        public ICollection<string> AllowedScopes { get; init; } = [];
    }
}
