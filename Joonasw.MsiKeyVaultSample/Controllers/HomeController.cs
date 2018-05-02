using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Joonasw.MsiKeyVaultSample.Models;
using Microsoft.Extensions.Configuration;

namespace Joonasw.MsiKeyVaultSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();
            string keyVaultUrl = _configuration["KeyVault:Url"];
            if (!string.IsNullOrEmpty(keyVaultUrl))
            {
                string secret = _configuration["Secret:Setting"];
                model.Message = $"Value gotten from Key Vault: {secret}";
            }
            else
            {
                model.Message = "Key Vault URL not found in configuration";
            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
