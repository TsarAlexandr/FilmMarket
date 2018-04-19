using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProject.Models
{
    public interface IRepository
    {
        List<Film> Films { get; }
        bool deleteFilm(Film film);
        bool addFilm(Film film);
        bool updateFilm(Film film);
        Film getFilmById(int? id);
    }
}
