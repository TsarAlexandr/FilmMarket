using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProject.Models
{
    [Serializable]
    public class Cart
    {
        private List<CartLine> lines;
        
        public void AddItem(Film film, int quantity)
        {
            var res = lines.FirstOrDefault(x => x.MyFilm.ID == film.ID);
            if (res == null)
            {
                lines.Add(new CartLine() { MyFilm = film, Quantity = quantity });
            }
            else {
                res.Quantity += quantity;
            }
        }
        public void RemoveLine(Film film)
        {
            var res = lines.FirstOrDefault(x => x.MyFilm.ID == film.ID);
            if (res.Quantity > 1)
                res.Quantity--; 
            else
                lines.RemoveAll(l => l.MyFilm.ID == film.ID);
        }

        public double ComputeTotalValue()
        {
            return lines.Sum(e => Double.Parse(e.MyFilm.Price) * e.Quantity);

        }
        public void Clear()
        {
            lines.Clear();
        }

        public List<CartLine> Lines
        {
            get {
                return lines;
            }
            set {
                if (lines == null)
                {
                    lines = value;
                }
            }
        }

    }

    public class CartLine
    {
        public Film MyFilm { get; set; }
        public int Quantity { get; set; }
    }
}
