using System;
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
    public class RestaurantsController : Controller
    {
        private ProjectContext db = new ProjectContext();


        // GET: Restaurants
        public ActionResult Index()
        {
            var restaurants = db.Restaurants;

            IOrderedEnumerable<Restaurant> restaurantList =
                restaurants.ToList().OrderByDescending(item => item.Rate).
                    OrderByDescending(item => RestaurantInterest.Instance.getInterestInRestaurant(Request, Response, item.RestaurantID));

            return View(restaurantList);
        }

        // GET: Restaurants/Locations
        public ActionResult Locations()
        {
            return View(db.Restaurants.ToList());
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = db.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            RestaurantInterest.Instance.AddUserInterestInRestaurant(Request, Response, restaurant.RestaurantID, 1);

            return View(restaurant);
        }

		// GET: Restaurants/GroupByArea
        [Authorize(Users = "Admin")]
        public ActionResult GroupByArea()
        {
            var totalAreas = from restaurant in db.Restaurants
                             group restaurant by restaurant.Area into g
                             select new GroupByAreaModel() { Area = g.Key, TotalRestaurants = g.Sum(p => 1) };

            return View(totalAreas.ToList());
        }

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult GroupByAreaData()
        {
            var totalAreas = from restaurant in db.Restaurants
                             group restaurant by restaurant.Area into g
                             select new GroupByAreaModel() { Area = g.Key, TotalRestaurants = g.Sum(p => 1) };

            return Json(totalAreas.ToList(), JsonRequestBehavior.AllowGet);
        }
		
        // POST: Restaurants/Filter
        public ActionResult Filter(string area, string restaurantName, int greaterRateThan)
        {
            var results = db.Restaurants.AsQueryable();
            Area areaE;

            if (!string.IsNullOrEmpty(restaurantName))
            {
                results = results.Where(restaurant => restaurant.Name.ToLower().IndexOf(restaurantName.ToLower()) > -1);
            }

            if (!string.IsNullOrEmpty(area) && Enum.TryParse(area, out areaE))
            {
                results = results.Where(restaurant => restaurant.Area == areaE);
            }

            results = results.Where(p => p.Rate > greaterRateThan);

            return View("Index", results.ToList());
        }
    }
}