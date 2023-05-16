using Barcode_Generator.Models;
using BarcodeLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using System.Reflection.Emit;


namespace Barcode_Generator.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
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
    //BarCode
    public IActionResult GenerateBarCode(string code = "112233")
    {
        Barcode barcode = new Barcode();
        Image img = barcode.Encode(TYPE.CODE39, code, Color.Black, Color.White, 250, 100);
        var data = ConvertImageToBytes(img);
        return File(data, "image/jpeg");
    }

    private byte[] ConvertImageToBytes(Image img)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            img.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }
    }
    //QRCode
    public IActionResult GenerateQRCode(string code = "Welcome")
    {
        QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
        QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        QRCode qRCode = new QRCode (qRCodeData);
        Bitmap bitmap = qRCode.GetGraphic(15);
        var bitmapBytes = ConvertBitmapToBytes(bitmap);
        return File(bitmapBytes, "image/jpeg");
    }

    private byte[] ConvertBitmapToBytes(Bitmap bitmap)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            bitmap.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }
    }

}

