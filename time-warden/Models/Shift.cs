using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace time_warden.Models
{
    public class Shift
    {
        public int ShiftId { get; set; }

        [Display(Name = "Clock In Time")]
        public DateTime ClockInTime { get; set; }

        [Display(Name = "Clock Out Time")]
        public DateTime ClockOutTime { get; set; }

        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; } //changed to decimal from timespan

        [Display(Name = "Shift Worked?")]
        public bool IsWorked { get; set; }

        public DateTime ShiftDate {  get; set; }

        //Navigational Properties - These are properties which are related to other classes and the basis of their relationships
        public string UserId { get; set; }
        public User User { get; set; }
        public readonly DBWriter DbWriter = new DBWriter();

        public Shift()
        {
            
        }

        public Shift ClockIn(User user)
        {
            Shift shift = new Shift();
            if (user != null)
            {
                shift.UserId = user.UserId; //Set the user ID properly
                shift.ClockInTime = DateTime.Now;
        
                // Call the DB method to start the shift
                DbWriter.StartShift(shift);
            }
            else
            {
                Console.WriteLine("Error: User object is null.");
            }

            return shift;
        }

        public Shift ClockOut(Shift shift)
        {
            shift.ClockOutTime = DateTime.Now;

            // Convert TimeSpan to decimal hours
            shift.HoursWorked = (decimal)(shift.ClockOutTime - shift.ClockInTime).TotalHours;

            DbWriter.EndShift(shift);
            return shift;
        }

        //public Shift ClockOut(Shift shift)
        //{
        //    shift.ClockOutTime = DateTime.Now;
        //    shift.HoursWorked = ClockOutTime - ClockInTime;
            
        //    DbWriter.EndShift(shift);
        //    return shift;
        //}
    }
}