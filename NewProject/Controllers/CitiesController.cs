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

namespace NewProject.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cities
        [Authorize(Roles ="admin")]
        public IActionResult Index()
        {
            return View(_context.Cities.ToList());
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,District,City")] Cities cities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cities);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(cities);
        }


        // GET: Cities/Edit/5
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cities = _context.Cities.FirstOrDefault(m => m.ID == id);
            if (cities == null)
            {
                return NotFound();
            }
            return View(cities);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,District,City")] Cities cities)
        {
            if (id != cities.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cities);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitiesExists(cities.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cities);
        }

        // GET: Cities/Delete/5
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cities = _context.Cities
                .FirstOrDefault(m => m.ID == id);
            if (cities == null)
            {
                return NotFound();
            }

            return View(cities);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cities = _context.Cities.FirstOrDefault(m => m.ID == id);
            _context.Cities.Remove(cities);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CitiesExists(int id)
        {
            return _context.Cities.Any(e => e.ID == id);
        }
    }
}
