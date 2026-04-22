namespace AuthServer.Models
{
    public record LoginDto
    {
        public string UserName { get; init; } = null!;
        public string Password { get; init; } = null!;
    }

    public record RegisterDto
    {
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }

    public record ConnectDto
    {
        public string ClientId { get; init; } = null!;
        public string ClientSecret { get; init; } = null!;
    }

    public record ClientConfigDto
    {
        public string ClientId { get; init; } = null!;
        public string ClientSecret { get; init; } = null!;
        public string GrantType { get; init; } = null!;
        public ICollection<string> AllowedScopes { get; init; } = [];
    }
}
