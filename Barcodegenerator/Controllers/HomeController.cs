using Barcodegenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBarcode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing.QrCode;
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
            try
            {
                string finalimg = string.Empty;
                var content = model.BarcodeValue;
                var width = 50;
                var height = 50;
                var barcodeWriterPixelData = new ZXing.BarcodeWriterPixelData
                {
                    Format = ZXing.BarcodeFormat.UPC_A,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = 0
                    }
                };
                var pixelData = barcodeWriterPixelData.Write(content);
                using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        try
                        {
                            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        finalimg = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(memoryStream.ToArray()));


                    }
                }
                ViewBag.BaseImage = finalimg;
                ViewBag.CodeValue = model.BarcodeValue;
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.Exception = ex.Message;
                return View();
            }
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
