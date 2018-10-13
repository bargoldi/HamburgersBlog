using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HamburgersBlog.DAL;
using HamburgersBlog.Models;

namespace HamburgersBlog.Controllers
{
    public class PostsController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            // Incerment the counter for this restaurant
            int restaurantId = post.Restaurant.RestaurantID;
            string restaurantName = db.Restaurants.First(restaurant => restaurant.RestaurantID == restaurantId).Name;
            Dictionary<string, int> counterDictionary = (Dictionary<string, int>)Session["counterDictionary"];
            if (counterDictionary == null)
            {
                counterDictionary = new Dictionary<string, int>();
            }
            if (counterDictionary.ContainsKey(restaurantName))
            {
                counterDictionary[restaurantName]++;
            }
            else
            {
                counterDictionary.Add(restaurantName, 1);
            }
            Session["counterDictionary"] = counterDictionary;

            RestaurantInterest.Instance.AddUserInterestInRestaurant(Request, Response, restaurantId, 1);

            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            var allRestaurants = db.Restaurants;
            this.ViewData["restaurantsSelectable"] = (IEnumerable<HamburgersBlog.Models.Restaurant>)allRestaurants;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Title,AuthorName,RestaurantID,Content")] Post post)
        {
            post.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();

                RestaurantInterest.Instance.AddUserInterestInRestaurant(Request, Response, post.RestaurantID, 5);

                // Find from text if it's a positive review
                bool isRecomended =
                           RestuarantRecomandationByNLP.Instance.IsPositiveReview(Request, Response, post.Title + " " + post.Content);

                // Change restuarnt recomendation
                SaveIsRecomendedForRestuarant(post.RestaurantID, isRecomended);

                return RedirectToAction("../Home/Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var allRestaurants = db.Restaurants;
            this.ViewData["restaurantsSelectable"] = (IEnumerable<HamburgersBlog.Models.Restaurant>)allRestaurants;
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult Edit([Bind(Include = "PostID,Title,AuthorName,RestaurantID,Restaurant,Date,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Date = DateTime.Now;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();

                // Find from text if it's a positive review
                bool isRecomended =
                    RestuarantRecomandationByNLP.Instance.IsPositiveReview(Request, Response, post.Title + " " + post.Content);

                // Change restuarnt recomendation
                SaveIsRecomendedForRestuarant(post.RestaurantID, isRecomended);

                return RedirectToAction("../Home/Index");
            }
            var allRestaurants = db.Restaurants;
            this.ViewData["restaurantsSelectable"] = (IEnumerable<HamburgersBlog.Models.Restaurant>)allRestaurants;
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("../Home/Index");
        }

        // POST: Posts/Filter
        public ActionResult Filter(string area, string restaurantName, string authorName)
        {
            var results = db.Posts.AsQueryable();
            Area areaE;

            if (!string.IsNullOrEmpty(restaurantName))
            {
                results = from post in db.Posts
                          join restaurant in db.Restaurants on post.RestaurantID equals restaurant.RestaurantID
                          where restaurant.Name.ToLower().Contains(restaurantName.ToLower())
                          select post;
            }

            if (!string.IsNullOrEmpty(area) && Enum.TryParse(area, out areaE))
            {
                results = results.Where(post => post.Restaurant.Area == areaE);
            }

            if (!string.IsNullOrEmpty(authorName))
            {
                results = results.Where(post => post.AuthorName.ToLower().IndexOf(authorName.ToLower()) > -1);
            }


            return View("Index", results.ToList());
        }

        private void SaveIsRecomendedForRestuarant(int restId, bool isRecomended)
        {
            Restaurant restaurant = db.Restaurants.Find(restId);
            restaurant.RecommendationScore += isRecomended ? 1 : -1;
            db.Entry(restaurant).State = EntityState.Modified;
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
