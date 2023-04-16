using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Mail.Tests
{
    internal class TestConfigHelper
    {
        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();
        }
    }
}
