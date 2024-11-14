using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using time_warden.Models;

namespace time_warden.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<User> users = new List<User>();
            DBReader db = new DBReader();

            users = db.GetEmployees();

            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine("Name: " +users[i].FirstName + " " + users[i].Surname);
            }
            
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}