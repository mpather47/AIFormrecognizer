using AIFormrecognizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;



namespace AIFormrecognizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly AiformDbContext _db;
        private readonly ILogger<HomeController> _logger;
        [TempData]
        public string Message { get; set; }
        [TempData]
        public string JsonData { get; set; }

        public HomeController(AiformDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }
       
        public  IActionResult  Index(Files file)
        {
            TempData["name"] = file.Filename;
            if (file.Filename != null)
            {
                return RedirectToAction("Output");
            }

            ViewBag.Output = JsonData;

            ViewModels mymodel = new ViewModels();
            mymodel.Invoices = _db.Invoices.ToArray();

            return View(mymodel);
        }

    
        public IActionResult ReturnJson()
        {

            return this.Json(new { data = JsonData });
        }

        public async Task<Invoice> Output(Formrecognizer formrecognizer)
        {
            var recognizerClient = formrecognizer.AuthenticateClient();

            _db.Invoices.Add(await formrecognizer.AnalyzeInvoice(recognizerClient, TempData["name"] as string));
            _db.SaveChanges();


            return  await formrecognizer.AnalyzeInvoice(recognizerClient, TempData["name"] as string );

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
