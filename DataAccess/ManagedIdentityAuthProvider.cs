using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.SqlServer.Dac;

namespace DacShipper
{
    public class ManagedIdentityAuthProvider : IUniversalAuthProvider
    {
        private readonly AzureServiceTokenProvider _managedIdentityTokenProvider = new AzureServiceTokenProvider();

        public string GetValidAccessToken() => _managedIdentityTokenProvider.GetAccessTokenAsync("https://database.windows.net/", "72f988bf-86f1-41af-91ab-2d7cd011db47").Result;
    }
}
