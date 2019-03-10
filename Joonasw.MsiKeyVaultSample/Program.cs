using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Joonasw.MsiKeyVaultSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    //Build the config from sources we have
                    var config = builder.Build();

                    string keyVaultUrl = config["KeyVault:Url"];
                    //Check a Key Vault has been configured
                    if (!string.IsNullOrEmpty(keyVaultUrl))
                    {
                        //Create Managed Service Identity token provider
                        var tokenProvider = new AzureServiceTokenProvider();
                        //Create the Key Vault client
                        var kvClient = new KeyVaultClient((authority, resource, scope) => tokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                        //Add Key Vault to configuration pipeline
                        builder.AddAzureKeyVault(config["KeyVault:Url"], kvClient, new DefaultKeyVaultSecretManager());
                    }
                });
    }
}
