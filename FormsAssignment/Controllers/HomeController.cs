using System;
using System.Diagnostics;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using FormsAssignment.Models;
using FormsAssignment.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FormsAssignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly ILogger<HomeController> _logger;
        private readonly IConverter _converter;

        public HomeController(IHostingEnvironment hostingEnv, ILogger<HomeController> logger, IConverter converter)
        {
            _hostingEnv = hostingEnv;
            _logger = logger;
            _converter = converter;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            string ext = Path.GetExtension(user.Proof.FileName);
            if (ext.ToLower() != ".pdf")
            {
                return View();
            }
            var path = Path.Combine(_hostingEnv.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //upload files to wwwroot
            var fileName = Path.GetFileName(user.Proof.FileName);
            using (FileStream streamToCopy = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                user.Proof.CopyTo(streamToCopy);
            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(user, Path.Combine(path, fileName)),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);
            return File(file, "application/pdf", $"{user.Fullname}.pdf");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
