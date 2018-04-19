using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewProject.Models;

namespace NewProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View("About");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View("Contact");
        }

        //-------------------------------------------------------------
        public IActionResult FormCalculator()
        {
            return View("FormCalculator");
        }

        public IActionResult Planets()
        {
            return View("Planets");
        }
        public IActionResult DigArr()
        {
            return View("DigArr");
        }
        public IActionResult StrPrg()
        {
            return View("StrPrg");
        }

        public IActionResult Navigator()
        {
            return View("Navigator");
        }
        //------------------------------------------------------------

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
