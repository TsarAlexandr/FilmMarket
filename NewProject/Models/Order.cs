using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewProject.Models
{
    public class Order
    {
        public int ID { get; set; }
        public Districts District { get; set; }
        public Cities City { get; set; } 
        [Required]
        [MaxLength(20)]
        public string Adress { get; set; }

    }

    public enum Districts
    {
        Minsk,
        Brest,
        Vitebsk,
        Gomel,
        Mogilev,
        Grodno
    }
    public class Cities
    {
        public int ID { get; set; }
        public Districts District { get; set; }
        [Required]
        [MaxLength(20)]
        public string City { get; set; }
    }
}
