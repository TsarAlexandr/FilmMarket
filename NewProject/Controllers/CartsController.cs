using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewProject.Models;
using NewProject.Data;
using Microsoft.AspNetCore.Authorization;

namespace NewProject.Controllers
{
    public class CartsController : Controller
    {
        private IRepository repo;

        public CartsController(IRepository repos)
        {
            repo = repos;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(GetCart());
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddFilm(int filmID)
        {
            var film = repo.getFilmById(filmID);
            if (film != null)
            {
                var cart = GetCart();
                cart.AddItem(film, 1);
                HttpContext.Session.Set("Cart", cart);
            }
            
            return RedirectToAction("Index", "Carts");

        }
        [Authorize]
        [HttpPost]
        public IActionResult RemoveFilm(int filmID)
        {
            var film = repo.getFilmById(filmID);
            if (film != null)
            {
                GetCart().RemoveLine(film);
            }
            return RedirectToAction("Index");

        }

        public Cart GetCart()
        {
            Cart cart = HttpContext.Session.Get<Cart>("Cart");
            if (cart == null)
            {
                cart = new Cart();
                cart.Lines = new List<CartLine>();                
            }
            return cart;
        }
    }
}