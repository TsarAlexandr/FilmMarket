using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewProject.Models;
using NewProject.Data;

namespace NewProject.Controllers
{
    public class CartsController : Controller
    {
        private IRepository repo;

        public CartsController(IRepository repos)
        {
            repo = repos;
        }
        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost]
        public IActionResult AddFilm(int filmID)
        {
            var film = repo.getFilmById(filmID);
            if (film != null)
            {
                GetCart().AddItem(film, 1);
            }
            return RedirectToAction("Index", "Films");

        }

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
                HttpContext.Session.Set("Cart", cart);
            }
            return cart;
        }
    }
}