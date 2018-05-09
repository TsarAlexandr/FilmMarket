using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewProject.Data;
using NewProject.Models;
using Newtonsoft.Json;

namespace NewProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public JsonResult getCity(int id)
        {
            Districts districts = (Districts)Enum.GetValues(typeof(Districts)).GetValue(id);
            List<Cities> list = new List<Cities>();
            list = _context.Cities.Where(city => city.District == districts).ToList();
            return Json(new SelectList(list, "ID", "City"));
        }



        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateCity()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCity([Bind("ID,District,City")] Cities cities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cities);
                _context.SaveChanges();
                
            }
            return View(cities);
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
