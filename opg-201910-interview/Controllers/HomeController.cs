using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using opg_201910_interview.Models;
using opg_201910_interview.Services.Contract;
using System.Diagnostics;
using System.IO;

namespace opg_201910_interview.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration { get; }
        private ILogger<HomeController> _logger;
        private IClientDirectoryService _clientDirectoryService;

        public HomeController(
            ILogger<HomeController> logger, 
            IConfiguration configuration,
            IClientDirectoryService clientDirectoryService)
        {
            _logger = logger;
            _configuration = configuration;
            _clientDirectoryService = clientDirectoryService;
        }

        public IActionResult Index()
        {
            IConfigurationSection clientSettings = _configuration.GetSection("ClientSettings");

            _clientDirectoryService.ClientId = clientSettings.GetSection("ClientId").Value;
            _clientDirectoryService.DirectoryPath = clientSettings.GetSection("FileDirectoryPath").Value;
            _clientDirectoryService.LoadDirectoryFiles("*.*", SearchOption.TopDirectoryOnly);
            _clientDirectoryService.SortDirectoryFilesByNameAndDate();

            if (clientSettings.GetSection("ClientId").Value == "1001")
                _clientDirectoryService.SortUniqueMechanismOne();
            else
                _clientDirectoryService.SortUniqueMechanismTwo();

            ViewData["ClientId"] = clientSettings.GetSection("ClientId").Value;

            return View(_clientDirectoryService.DirectoryFileList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
