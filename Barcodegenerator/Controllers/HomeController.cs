using Barcodegenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBarcode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
//ref https://github.com/Tagliatti/NetBarcode
namespace Barcodegenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //var barcode = new Barcode("543534");
            //var value = barcode.GetBase64Image();
            //ViewBag.BaseImage = value;
            return View();
        }

        [HttpPost]
        public IActionResult Index( BarcodeModel model)
        {
           // var barcode = new Barcode(model.BarcodeValue);
            var barcode = new Barcode(model.BarcodeValue, NetBarcode.Type.EAN13, true);
            var value = barcode.GetBase64Image();
            ViewBag.BaseImage = value;
            ViewBag.CodeValue = model.BarcodeValue;
            return View();
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
