using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HamburgersBlog.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public int RestaurantID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class GroupByRestaurantModel
    {
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public int TotalPosts { get; set; }
    }
}