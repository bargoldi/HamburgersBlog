using HamburgersBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HamburgersBlog.DAL
{
    public class ProjectInitializer : System.Data.Entity.DropCreateDatabaseAlways<ProjectContext>
    {
        protected override void Seed(ProjectContext context)
        {
            var hamburgers = new List<Hamburger>
            {
                new Hamburger
                {
                    HamburgerID=1,
                    Name="Cheesy Bacon",
                    Description="Tons of Cheese and Bacon",
                    Price=55,
                },
                new Hamburger
                {
                    HamburgerID=2,
                    Name="Butler Burger",
                    Description="Classic burger",
                    Price=57,
                },
                new Hamburger
                {
                    HamburgerID=3,
                    Name="Butler Cheese Burger",
                    Description="Classic burger with cheese and a love :)",
                    Price=60,
                },
                new Hamburger
                {
                    HamburgerID=4,
                    Name="Thailand",
                    Description="Rice Pasta and Pinapple",
                    Price=60,
                },
                new Hamburger
                {
                    HamburgerID=5,
                    Name="Fini",
                    Description="Maple Sirup and Bacon!",
                    Price=60,
                },
                new Hamburger
                {
                    HamburgerID=6,
                    Name="Sweet Eggs",
                    Description="Sweet onion jam and eggs",
                    Price=60,
                },
                new Hamburger
                {
                    HamburgerID=7,
                    Name="Sweet Crabs",
                    Description="Sweet wine marinade with crabs",
                    Price=60,
                },
                new Hamburger
                {
                    HamburgerID=8,
                    Name="Blue and Smoked",
                    Description="Blue cheese, Bacon, Smoked Onion",
                    Price=60,
                },
                new Hamburger
                {
                    HamburgerID=9,
                    Name="Royal with Cheese",
                    Description="Hot Cheese FONDU with Bacon!",
                    Price=60,
                },
            };

            hamburgers.ForEach(h => context.Hamburgers.Add(h));
            context.SaveChanges();

            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    RestaurantID=1,
                    Location="Tel Aviv",
                    Name="Vitrina",
                    Rate=5,
                    IsKosher=false,
                    IsParkingAvailable=false,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                    Hamburgers=new List<Hamburger>{hamburgers[1], hamburgers[2]}
                },
                new Restaurant
                {
                    RestaurantID=2,
                    Location="Rishon Lezion",
                    Name="SuSu And Sons",
                    Rate=2,
                    IsKosher=true,
                    IsParkingAvailable=true,
                    IsVeganFriendly=false,
                    Area=Area.Hamerkaz,
                },
                new Restaurant
                {
                    RestaurantID=3,
                    Location="Netanya",
                    Name="Humongous",
                    Rate=4,
                    IsKosher =false,
                    IsParkingAvailable=true,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                    Hamburgers=new List<Hamburger>{hamburgers[3], hamburgers[4]}
                },
                new Restaurant
                {
                    RestaurantID=4,
                    Location="Tel Aviv",
                    Name="Prozdor",
                    Rate=5,
                    IsKosher=false,
                    IsParkingAvailable=false,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                    Hamburgers=new List<Hamburger>{hamburgers[7], hamburgers[8]}
                },
                new Restaurant
                {
                    RestaurantID=5,
                    Location="Tel Aviv",
                    Name="Port19",
                    Rate=3,
                    IsKosher=false,
                    IsParkingAvailable=true,
                    IsVeganFriendly=false,
                    Area=Area.Hadarom,
                    Hamburgers=new List<Hamburger>{hamburgers[6]}
                },
                new Restaurant
                {
                    RestaurantID=6,
                    Location="Rishon Letzion",
                    Name="BBB",
                    Rate=4,
                    IsKosher=false,
                    IsParkingAvailable=true,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                    Hamburgers=new List<Hamburger>{hamburgers[0]}
                },
                new Restaurant
                {
                    RestaurantID=7,
                    Location="Holon",
                    Name="Gordos",
                    Rate=2,
                    IsKosher=false,
                    IsParkingAvailable=true,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                },
                new Restaurant
                {
                    RestaurantID=8,
                    Location="Rishon Letzion",
                    Name="Kabana",
                    Rate=1,
                    IsKosher=false,
                    IsParkingAvailable=true,
                    IsVeganFriendly=true,
                    Area=Area.Hadarom,
                    Hamburgers=new List<Hamburger>{hamburgers[5]}
                },
            };

            restaurants.ForEach(r => context.Restaurants.Add(r));
            context.SaveChanges();

            var reviews = new List<Review>
            {
                new Review
                {
                    ReviewID=1,
                    Title="very tasty",
                    AuthorName="Shlomi",
                    Content="best burger in town",
                    RestaurantID=1,
                },
                new Review
                {
                    ReviewID=2,
                    Title="yum yum",
                    AuthorName="Itzik a gadol",
                    Content="Wow, what a burger!",
                    RestaurantID=2,
                },
                new Review
                {
                    ReviewID=2,
                    Title="Too  expensive",
                    AuthorName="Young Itzik",
                    Content="I like the burger but the price is too high!",
                    RestaurantID=3,
                },
                new Review
                {
                    ReviewID=3,
                    Title="WoW",
                    AuthorName="Noa",
                    Content="There is no doubt that this burger in my top 5",
                    RestaurantID=1,
                },
            };

            reviews.ForEach(r => context.Reviews.Add(r));
            context.SaveChanges();

            var posts = new List<Post>
            {
                new Post {
                    PostID = 1,
                    Title = "Why Vetrina is the BEST Burger Ever Made!",
                    AuthorName = "Bar Goldinfeld",
                    Date=new DateTime(2018, 5, 9),
                    Content="Cuz. it's made with love!",
                    RestaurantID=restaurants[0].RestaurantID,
                },
                new Post  {
                    PostID = 2,
                    Title = "I Wish I Had My Own Restaurant - Just Like Kabana",
                    AuthorName="Ron Badur",
                    Date=new DateTime(2018,5,2),
                    Content="Courage is needed for that mission! There are so many unsuccessful Burgers",
                    RestaurantID=restaurants[7].RestaurantID,
                },
                new Post  {
                    PostID = 3,
                    Title = "The Truth About Susu and Son",
                    AuthorName="Roee Rokah",
                    Date=new DateTime(2018,4,30),
                    Content="Jesus so much HYPE around it, but let's face it - it's a bad Burger.",
                    RestaurantID=restaurants[1].RestaurantID,
                }
            };

            posts.ForEach(p => context.Posts.Add(p));
            context.SaveChanges();

            var comments = new List<Comment>
            {
                new Comment
                {
                    CommentID=1,
                    PostID=1,
                    Title="Dam Right!",
                    AuthorName="IDontLoveBurgersXD",
                    Content="You just can't be wrong about that Burger",
                },
                new Comment
                {
                    CommentID=2,
                    PostID=1,
                    Title="You are wrong!",
                    AuthorName="HATERSGONNAHATE",
                    Content="Haters Gonna HATE!",
                },
                new Comment
                {
                    CommentID=3,
                    PostID=2,
                    Title="Kabana?!",
                    AuthorName="I'm a Pro XD",
                    Content="As a pro, you don't wanna learn from that shitty place ;)",
                },
            };

            comments.ForEach(c => context.Comments.Add(c));
            context.SaveChanges();
        }
    }
}