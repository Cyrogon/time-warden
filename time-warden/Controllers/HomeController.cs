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
            var todaysShift = dbReader.GetTodaysShift(loggedInUser.UserId); //Get todays shift using method in DBReader with userid passed in
            
            if (todaysShift == null)
            {
                //No shift found for today, pass a flag or null to the view
                ViewBag.Message = "You do not have a shift scheduled for today. If you believe you are supposed to be working, ask a manager to schedule you a shift.";
                ViewBag.CanClockIn = false;
                ViewBag.CanClockOut = false;
                return View(); //Return view without a shift
            }

            //Determine if the shift is scheduled, active, or completed
            if (todaysShift.Status == "Scheduled")
            {
                ViewBag.Message = $"You are scheduled to clock in for a shift today at {todaysShift.ClockInTime:HH:mm}.";
                ViewBag.CanClockIn = true;
                ViewBag.CanClockOut = false;
            }
            else if (todaysShift.Status == "Active")
            {
                ViewBag.Message = $"You are currently clocked in. You are due to clock out at {todaysShift.ClockOutTime:HH:mm}.";
                ViewBag.CanClockIn = false;
                ViewBag.CanClockOut = true;
            }
            else if (todaysShift.Status == "Complete")
            {
                ViewBag.Message = $"You finished your shift today. Total hours worked: {todaysShift.HoursWorked} hours.";
                ViewBag.CanClockIn = false;
                ViewBag.CanClockOut = false;
            }
            
            return View(todaysShift);
        }

        //Clock In action
        [HttpPost]
         public ActionResult ClockIn()
         {
             //Get the logged in user from the session
             var loggedInUser = (User)Session["LoggedInUser"];
        
             DBReader dbReader = new DBReader();
             var todaysShift = dbReader.GetTodaysShift(loggedInUser.UserId); //Check to see if there is a shift today
        
             if (todaysShift == null || todaysShift.Status != "Scheduled") //If user has no shift today, or status isn't scheduled, then can't clock in
             {
                 return RedirectToAction("Index");
             }
        
             //Clock in
             DBWriter dbWriter = new DBWriter();
             todaysShift.ClockInTime = DateTime.Now;
             todaysShift.Status = "Active";
             dbWriter.StartShift(todaysShift);
        
             TempData["SuccessMessage"] = $"Clocked in successfully at {todaysShift.ClockInTime:HH:mm:ss}!";
             return RedirectToAction("Index");
         }

        //Clock Out action
        [HttpPost]
        public ActionResult ClockOut()
        {
            DBReader dbReader = new DBReader();
            var loggedInUser = (User)Session["LoggedInUser"];
        
            //Get the current shift
            var activeShift = dbReader.GetActiveShift(loggedInUser.UserId);
            if (activeShift == null) 
            {
                TempData["ErrorMessage"] = "No ongoing shift to clock out of.";
                return RedirectToAction("Index");
            }
        
            Console.WriteLine($"Current Shift ID: {activeShift.ShiftId}, ClockInTime: {activeShift.ClockInTime}");
        
            //Clock out the shift
            activeShift = activeShift.ClockOut(activeShift); //Call the ClockOut method
            TempData["SuccessMessage"] = $"Clocked out successfully. Your shift started at {activeShift.ClockInTime:HH:mm:ss} and ended at {DateTime.Now:HH:mm:ss}.";
        
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