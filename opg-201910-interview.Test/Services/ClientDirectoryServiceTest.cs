using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using opg_201910_interview.Services.Concrete;
using opg_201910_interview.Services.Contract;
using System.IO;

namespace opg_201910_interview.Test.Services
{
    [TestClass]
    public class ClientDirectoryServiceTest
    {
        private IConfiguration _configuration;
        private IClientDirectoryService _clientDirectoryService;

        [TestInitialize]
        public void Setup()
        {
            _configuration = getConfig();

            ServiceCollection services = new ServiceCollection();
            services.AddTransient<IClientDirectoryService, ClientDirectoryService>();
            services.AddLogging();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            _clientDirectoryService = serviceProvider.GetService<IClientDirectoryService>();

            IConfigurationSection clientSettings = _configuration.GetSection("ClientSettings");
            _clientDirectoryService.ClientId = clientSettings.GetSection("ClientId").Value;
            _clientDirectoryService.DirectoryPath = clientSettings.GetSection("FileDirectoryPath").Value;
        }

        [TestMethod]
        public void LoadDirectoryFiles()
        {
            _clientDirectoryService.LoadDirectoryFiles("*", SearchOption.TopDirectoryOnly);

            Assert.IsNotNull(_clientDirectoryService.DirectoryFileList);
        }

        [TestMethod]
        public void SortDirectoryFilesByNameAndDate()
        {
            _clientDirectoryService.LoadDirectoryFiles("*", SearchOption.TopDirectoryOnly);
            _clientDirectoryService.SortDirectoryFilesByNameAndDate();

            Assert.IsNotNull(_clientDirectoryService.DirectoryFileList);
        }

        [TestMethod]
        public void SortUniqueMechanismOne()
        {
            _clientDirectoryService.LoadDirectoryFiles("*", SearchOption.TopDirectoryOnly);
            _clientDirectoryService.SortDirectoryFilesByNameAndDate();
            _clientDirectoryService.SortUniqueMechanismOne();

            Assert.IsNotNull(_clientDirectoryService.DirectoryFileList);
        }

        [TestMethod]
        public void SortUniqueMechanismTwo()
        {
            _clientDirectoryService.LoadDirectoryFiles("*", SearchOption.TopDirectoryOnly);
            _clientDirectoryService.SortDirectoryFilesByNameAndDate();
            _clientDirectoryService.SortUniqueMechanismTwo();

            Assert.IsNotNull(_clientDirectoryService.DirectoryFileList);
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
