using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HamburgersBlog.Models
{
    public enum Area
    {
        Hazafon, Hadarom, Hamerkaz,
    }

    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public Area Area { get; set; }
        public bool IsVeganFriendly { get; set; }
        public bool IsKosher { get; set; }
        public bool IsParkingAvailable { get; set; }

        public virtual ICollection<Hamburger> Hamburgers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }

    public class GroupByAreaModel
    {
        public Area Area { get; set; }
        public int TotalRestaurants { get; set; }
    }
}