using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using time_warden.Models;

namespace time_warden.Controllers
{
    [Authorize] //Authorize ensures user has to be logged in to reach these pages
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            //Get logged in user
            var loggedInUser = (User)Session["LoggedInUser"];
            if (loggedInUser == null)
            {
                //Handle the case where the user is not logged in (redirect to logout page to clear any previous user data held, which then redirects to login)
                return RedirectToAction("Logout", "Account");
            }

            DBReader dbReader = new DBReader();
            var currentShift =
                dbReader.GetCurrentShift(loggedInUser
                    .UserId); //Get current shift using method in DBReader with userid passed in

            return View(currentShift);
        }

        //Clock In action
        [HttpPost]
        public ActionResult ClockIn()
        {
            //Get the logged in user from the session
            var loggedInUser = (User)Session["LoggedInUser"];

            DBReader dbReader = new DBReader();
            var currentShift =
                dbReader.GetCurrentShift(loggedInUser.UserId); //Check to see if there is a current shift active

            if (currentShift != null &&
                currentShift.ClockOutTime == DateTime.MinValue) //If user has ongoing shift, can't click clockin
            {
                TempData["ErrorMessage"] = $"You already have an ongoing shift started at {currentShift.ClockInTime}";
                return RedirectToAction("Index");
            }

            //Create a new shift and clock in
            Shift shift = new Shift();
            shift = shift.ClockIn(loggedInUser); //Call the ClockIn method form shift

            TempData["SuccessMessage"] = $"Clocked in successfully at {shift.ClockInTime:HH:mm:ss}!";

            //Redirect to the Index page 
            return RedirectToAction("Index");
        }

        //Clock Out action
        [HttpPost]
        public ActionResult ClockOut()
        {
            DBReader dbReader = new DBReader();
            var loggedInUser = (User)Session["LoggedInUser"];

            //Get the current shift
            var currentShift = dbReader.GetCurrentShift(loggedInUser.UserId);
            if (currentShift == null) //|| currentShift.ClockInTime == DateTime.MinValue 
            {
                TempData["ErrorMessage"] = "No ongoing shift to clock out of.";
                return RedirectToAction("Index");
            }

            Console.WriteLine($"Current Shift ID: {currentShift.ShiftId}, ClockInTime: {currentShift.ClockInTime}");

            //Clock out the shift
            currentShift = currentShift.ClockOut(currentShift); //Call the ClockOut method
            TempData["SuccessMessage"] =
                $"Clocked out successfully. Your shift started at {currentShift.ClockInTime:HH:mm:ss} and ended at {DateTime.Now:HH:mm:ss}.";

            //Redirect to the Index page
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var loggedInUser = (User)Session["LoggedInUser"];
            DBReader dbReader = new DBReader();
            List<Shift> shifts = dbReader.GetEmployeeShifts(loggedInUser);

            foreach (var shift in shifts)
            {
                Console.WriteLine(shift.ShiftId +" " + shift.UserId);
            }

            return View();
        }
        
        

    }
}