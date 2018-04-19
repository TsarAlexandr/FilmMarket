using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewProject.Models;

namespace NewProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,IRepository
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public bool deleteFilm(Film film)
        {
            this.Film.Remove(film);
            this.SaveChanges();
            return true;
        }

        public bool addFilm(Film film)
        {
            this.Film.Add(film);
            this.SaveChanges();
            return true;
            
        }

        public bool updateFilm(Film film)
        {
            this.Film.Update(film);
            this.SaveChanges();
            return true;
             
        }

        public Film getFilmById(int? id)
        {
            return this.Film.SingleOrDefault(m => m.ID == id);
        }

        public DbSet<NewProject.Models.Film> Film { get; set; }

        public List<Film> Films => this.Film.ToList();
    }
}
