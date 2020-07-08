using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using opg_201910_interview.Controllers;
using opg_201910_interview.Services.Concrete;
using opg_201910_interview.Services.Contract;
using System.IO;

namespace opg_201910_interview.Test
{
    [TestClass]
    public class HomeControllerTest
    {

        private IConfiguration _configuration;
        private ILogger<HomeController> _logger;
        private IClientDirectoryService _clientDirectoryService;
        private HomeController _homeController;

        [TestInitialize]
        public void Setup()
        {
            _configuration = getConfig();

            ServiceCollection services = new ServiceCollection();
            services.AddTransient<IClientDirectoryService, ClientDirectoryService>();
            services.AddLogging();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            _clientDirectoryService = serviceProvider.GetService<IClientDirectoryService>();
            _logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<HomeController>();

            _homeController = new HomeController(_logger, _configuration, _clientDirectoryService);
        }

        [TestMethod]
        public void Index()
        {
            var result = _homeController.Index() as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        private static IConfiguration getConfig()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(System.Environment.CurrentDirectory.Replace(".Test", string.Empty))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return configBuilder.Build();
        }
    }
}
