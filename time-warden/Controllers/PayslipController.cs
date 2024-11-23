using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Org.BouncyCastle.Asn1.X509;
using time_warden.Models;

namespace time_warden.Controllers
{
    public class PayslipController : Controller
    {
        // GET: Payslip
        public ActionResult Index()
        {
            return View();
        }

        // GET: Payslip/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Payslip/Create
        public ActionResult Create()
        {
            var loggedInUser = (User)Session["LoggedInUser"];
            DBReader reader = new DBReader();
            
            List<Shift> shifts = reader.GetShiftsForSlip(loggedInUser);

            decimal totalHours = 0;
            
        //tally up all hours worked this calendar month
        //once db is updated it will exclude shifts which were not worked but include upcoming shifts
        //in order to provide an idea of the final amount
            foreach (Shift shift in shifts)
            {
                totalHours += shift.HoursWorked;
            }
            
            Payslip payslip = new Payslip();
            payslip.Shifts = shifts;
            payslip.TotalHoursWorked = totalHours;
            payslip.UserId = loggedInUser.UserId;
            
            //until db is updated, this will be fixed rate of pay
            payslip.TotalPay = totalHours*11;
            
            //can remove this when doing frontend, this is for testing
            
            Console.WriteLine(payslip.TotalHoursWorked);
            Console.WriteLine(payslip.TotalPay);
            
            return View();
        }

        // POST: Payslip/Create
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

        // GET: Payslip/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Payslip/Edit/5
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

        // GET: Payslip/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Payslip/Delete/5
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
