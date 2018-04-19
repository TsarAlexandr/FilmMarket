using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewProject.Data;
using NewProject.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace NewProject.Controllers
{
    public class FilmsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private IRepository repos;
        public FilmsController(IRepository repo)
        {
            repos = repo;
        }

        // GET: Films
        public IActionResult Index()
        {
            return View(repos.Films);
        }
        public IActionResult Index1(String Search)
        {
            if (!String.IsNullOrEmpty(Search))
            {
                var list = repos.Films.Where(x => (x.Name.Contains(Search)));
                return View("Index", list);
            }

            return View("Index", repos.Films);
        }

            // GET: Films/Details/5
            public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = repos.getFilmById(id);
            if (film == null)
            {
                return NotFound();
            }

            return View("Details", film);
        }
        [Authorize(Roles ="admin")]
        // GET: Films/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Films/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Rating,Name,Description,Price,ImageMimeType")] Film film)
        {
            if (ModelState.IsValid)
            {
                if (film.ImageMimeType != null)
                {
                    byte[] imageData = null;
                    
                    using (var binaryReader = new BinaryReader(film.ImageMimeType.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)film.ImageMimeType.Length);
                    }
                    
                    film.ImageData = imageData;
                }
                repos.addFilm(film);
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Edit/5
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = repos.getFilmById(id);
            if (film == null)
            {
                return NotFound();
            }
            return View("Edit", film);
        }

        // POST: Films/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,Rating,Name,Description,Price")] Film film, IFormFile ImageMimeType)
        {
            if (id != film.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageMimeType != null)
                    {
                        byte[] imageData = null;

                        using (var binaryReader = new BinaryReader(ImageMimeType.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)ImageMimeType.Length);
                        }

                        film.ImageData = imageData;
                    }
                    repos.updateFilm(film);
                }
                catch (DbUpdateException)
                {
                    if (!FilmExists(film.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Delete/5
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = repos.getFilmById(id);
            if (film == null)
            {
                return NotFound();
            }

            return View("Delete", film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var film = repos.getFilmById(id);
            repos.deleteFilm(film);
            return RedirectToAction(nameof(Index));
        }

        //GET: Films/Buy/5
        [Authorize]
        public IActionResult Buy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = repos.getFilmById(id);
            if (film == null)
            {
                return NotFound();
            }

            return View("Buy", film);
        }

        public bool FilmExists(int id)
        {
            return repos.Films.Any(e => e.ID == id);
        }
    }
}
