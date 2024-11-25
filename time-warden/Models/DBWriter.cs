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
        public void StartShift(Shift shift) //Clocks in, updating the shift clock in time and status
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE employee_timesheet SET shift_start = @ClockInTime, status = @Status WHERE timesheet_id = @ShiftId";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        //parameterised methods for safety
                        cmd.Parameters.AddWithValue("@ClockInTime", shift.ClockInTime);
                        cmd.Parameters.AddWithValue("@Status", shift.Status);
                        cmd.Parameters.AddWithValue("@ShiftId", shift.ShiftId); // Ensure this parameter is passed
                        
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
                    string query = "UPDATE employee_timesheet SET shift_end = @ClockOutTime, hours_worked = @HoursWorked, status = @Status WHERE timesheet_id = @ShiftId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClockOutTime", shift.ClockOutTime);
                        cmd.Parameters.AddWithValue("@HoursWorked", shift.HoursWorked);
                        cmd.Parameters.AddWithValue("@Status", shift.Status);
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
        
        public void AddShift(Shift shift) //Used by managers when scheduling shifts
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO employee_timesheet (employee_id, date, shift_start, shift_end, status, hours_worked) 
                                                                    VALUES (@UserId, @ShiftDate, @ShiftStart, @ShiftEnd, @Status, 0);";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", shift.UserId);
                        cmd.Parameters.AddWithValue("@ShiftDate", shift.ShiftDate);
                        cmd.Parameters.AddWithValue("@ShiftStart", shift.ClockInTime.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@ShiftEnd", shift.ClockOutTime.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Status", shift.Status);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Data inserted successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}