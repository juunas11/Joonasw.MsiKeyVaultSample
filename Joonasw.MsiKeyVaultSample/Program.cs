using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging;

namespace Joonasw.MsiKeyVaultSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
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
                })
                .Build();
    }
}
