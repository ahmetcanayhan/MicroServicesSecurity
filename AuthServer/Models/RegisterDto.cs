namespace AuthServer.Models
{
    public record RegisterDto
    {
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
