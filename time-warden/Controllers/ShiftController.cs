using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using time_warden.Models;

namespace time_warden.Controllers
{
    [Authorize]
    public class ShiftController : Controller
    {
        // GET: Shift view all shifts for the logged in user
        public ActionResult Index()
        {
            var loggedInUser = (User)Session["LoggedInUser"];  //Get logged in user
            if (loggedInUser != null)
            {
                //Get the list of shifts for the logged-in user
                DBReader dbReader = new DBReader();
                List<Shift> shifts = dbReader.GetEmployeeShifts(loggedInUser);
                
                return View(shifts); //Pass the list of shifts to the view
            }
            else
            {
                return RedirectToAction("Logout", "Account");  //Redirect to login page
            }
        }
        
        //--------------------------------------------------
        // Get: ManageShifts
        public ActionResult ManageShifts()
        {
            var dbReader = new DBReader();

            //Get all shifts and employees with dbreader methods
            var shifts = dbReader.GetShifts();
            var employees = dbReader.GetEmployees();

            //Populate the User property for each shift
            foreach (var shift in shifts)
            {
                var employee = employees.FirstOrDefault(e => e.UserId == shift.UserId);
                if (employee != null)
                {
                    shift.User = employee;
                }
            }

            return View(shifts); // Pass the list of shifts (with populated User properties) to the view
            
            // DBReader dbReader = new DBReader();
            // List<Shift> shifts = dbReader.GetShifts(); //Get list of all employee shifts
            //
            // //Sort the list of shifts by ShiftDate in ascending order
            // var sortedShifts = shifts.OrderBy(s => s.ShiftDate).ToList();
            //
            // return View(sortedShifts); //Pass the list of shifts to the view
        }
        

        // GET: Shift/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Shift/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shift/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shift/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Shift/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shift/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Shift/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
