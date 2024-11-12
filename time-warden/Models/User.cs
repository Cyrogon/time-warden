using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace time_warden.Models
{
    public class User
    {
        public string UserId { get; set; }
        
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set;}

        [Required]
        public string Address { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Display(Name = "Date Hired")]
        [Required]
        public DateTime DateHired { get; set; }

        public string Department { get; set; }

        [Display(Name = "Date of Birth")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public string Mobile {  get; set; }

        [Display(Name = "Hourly Rate")]
        [Required]
        public decimal HourlyRate { get; set; }

        //Navigational Properties - These are properties which are related to other classes and the basis of their relationships
        public List<Shift> Shifts { get; set; }
    }
}