using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.SqlServer.Dac;

namespace DacShipper
{
    public class ManagedIdentityAuthProvider : IUniversalAuthProvider
    {
        // TODO: This assumes that ManagedIdentidyAuthProvider is a singleton.  If it's not, AzureServiceTokenProvider
        // should be injected as a singleton.
        private readonly AzureServiceTokenProvider _managedIdentityTokenProvider = new AzureServiceTokenProvider();

        // TODO: if your identity belongs in multiple directories, you'll need to provide that directory ID in the GetAccessTokenAsync call below.
        public string GetValidAccessToken() => _managedIdentityTokenProvider.GetAccessTokenAsync("https://database.windows.net/").Result;
    }
}
