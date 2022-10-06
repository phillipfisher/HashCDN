using HashCDN;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SampleWeb.Models;
using System.Diagnostics;

namespace SampleWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly CDN cdn;
        private readonly ILogger<HomeController> logger;

        public HomeController(CDN cdn, ILogger<HomeController> logger)
        {
            this.cdn = cdn;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(IndexViewModel vm)
        {
            if (vm.Data != null)
            {
                var res = await cdn.Store(vm.Data);

                vm.UploadMessage = res.Status.ToString();
                vm.UploadName = res.Name;
                vm.UploadUri = res.FullUri;
            }

            return View(vm);
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