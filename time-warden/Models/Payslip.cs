using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace time_warden.Models
{
    public class Payslip
    {
        public int PayslipId { get; set; }
        
        public decimal TotalHoursWorked { get; set; }

        public decimal TotalPay {  get; set; }


        //Navigational Properties - These are properties which are related to other classes and the basis of their relationships
        public List<Shift> Shifts { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}