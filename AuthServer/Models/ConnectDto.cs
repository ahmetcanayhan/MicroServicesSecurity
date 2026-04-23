namespace AuthServer.Models
{
    public record ConnectDto
    {
        public string ClientId { get; init; } = null!;
        public string ClientSecret { get; init; } = null!;
    }
}
