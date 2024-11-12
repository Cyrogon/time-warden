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
        public TimeSpan HoursWorked { get; set; }

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
            shift.UserId = user.UserId;
            shift.ClockInTime = DateTime.Now;

            DbWriter.StartShift(shift);

            return shift;
        }

        public Shift ClockOut(Shift shift)
        {
            shift.ClockOutTime = DateTime.Now;
            shift.HoursWorked = ClockOutTime - ClockInTime;
            
            DbWriter.EndShift(shift);
            return shift;
        }
    }
}