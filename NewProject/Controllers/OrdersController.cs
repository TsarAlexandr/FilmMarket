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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("District,Adress")]Order order, int city)
        {
            if (ModelState.IsValid)
            {
                order.City = _context.Cities.FirstOrDefault(x => x.ID == city);
                var cart = HttpContext.Session.Get<Cart>("Cart");
                if (cart != null)
                {
                    cart.Clear();
                    HttpContext.Session.Set("Cart", cart);
                }

                TempData["message"] = "Order Passed! Address:" + order.District.ToString() + " "
                    + order.City.City.ToString() + " " +
                    order.Adress.ToString();
                return RedirectToAction("Index", "Home");
            }
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.ID == id);
        }
    }
}
