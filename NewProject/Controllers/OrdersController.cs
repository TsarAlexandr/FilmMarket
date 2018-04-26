using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewProject.Data;
using NewProject.Models;

namespace NewProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        [HttpPost]
        public JsonResult GetCity(Districts district)
        {
            List<Cities> cities = _context.Cities.Where(city => city.District == district).ToList();
            return Json(cities);
        }
       


        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateCity()
        {
            return View("City");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCity([Bind("District","City")] Cities city)
        {
            if (ModelState.IsValid)
            {
                _context.Cities.Add(city);
            }
            return View("City");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        TempData = "Order Passed!";
        //    }
        //    return View(order);
        //}

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.ID == id);
        }
    }
}
