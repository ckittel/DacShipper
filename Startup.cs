using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Dac;

[assembly: FunctionsStartup(typeof(DacShipper.Startup))]

namespace DacShipper
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<DatabaseOptions>()
                            .Configure<IConfiguration>((settings, configuration) => configuration.Bind(settings));

            builder.Services.AddSingleton<IUniversalAuthProvider, ManagedIdentityAuthProvider>()
                            .AddSingleton<IDacServicesFactory, DacServicesFactory>();
        }
    }
}
