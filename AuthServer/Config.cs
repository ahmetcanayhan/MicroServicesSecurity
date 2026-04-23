using AuthServer.Models;
using Duende.IdentityServer.Models;

namespace AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> GetApiScopes(IConfiguration configuration) => configuration.GetSection("IdentityServerConfig:ApiScopes").Get<List<ApiScope>>() ?? [];

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var cfg = configuration.GetSection("IdentityServerConfig:ApiScopes:Clients").Get<List<ClientConfigDto>>() ?? [];

            var clients = new List<Client>();
            if (cfg != null)
            {
                foreach (var c in cfg)
                {
                    var granType = c.GrantType == "ClientCredentials" ? GrantTypes.ClientCredentials : GrantTypes.ResourceOwnerPassword;

                    clients.Add(new Client
                    {
                        ClientId = c.ClientId,
                        ClientSecrets = { new Secret(c.ClientSecret.Sha256()) },
                        AllowedScopes = c.AllowedScopes,
                        AllowedGrantTypes = granType
                    });
                }
            }
            return clients;
        }
    }
}
