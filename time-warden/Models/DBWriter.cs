using System;
using MySql.Data.MySqlClient;

namespace time_warden.Models
{
    public class DBWriter
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MariaDbConnection"].ConnectionString;
        
        /*StartShift and EndShift are separate database methods because we need for the shift
         to be saved as soon as it is opened. This allows other users to Log In and start shifts
         without any data being lost. The shift ending is treated as a record modification wherein
         end time and hours worked are added.
         */
        public void StartShift(Shift shift)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO employee_timesheet (employee_id, date, shift_start) VALUES (shift.UserId, CURRENT_DATE, shift.ClockInTime)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        //parameterised methods for safety
                        cmd.Parameters.AddWithValue("@UserId", shift.UserId);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Today);
                        cmd.Parameters.AddWithValue("@ClockInTime", shift.ClockInTime);
                        
                        
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Data inserted successfully.");
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    conn.Close();
                }
            }
        }

        public void EndShift(Shift shift)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE employee_timesheet SET shift_end = @ClockOutTime, hours_worked = @HoursWorked WHERE timesheet_id = @ShiftId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClockOutTime", shift.ClockOutTime);
                        cmd.Parameters.AddWithValue("@HoursWorked", shift.HoursWorked);
                        cmd.Parameters.AddWithValue("@ShiftId", shift.ShiftId);
                        
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Data inserted successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}