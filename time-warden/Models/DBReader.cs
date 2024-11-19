using System.Collections.Generic;

namespace time_warden.Models
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;
    using System.Configuration;

    public class DBReader
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MariaDbConnection"].ConnectionString;

        public List<User> GetEmployees()
        {
            List<User> employees = new List<User>();
            int i = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM employees e JOIN employee_logon el ON e.employee_id = el.employee_id;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Reading...");
                                
                                User employee = new User
                                {
                                    UserId = reader["employee_id"].ToString(),
                                    FirstName = reader["first_name"].ToString(),
                                    Surname = reader["last_name"].ToString(),
                                    Role = reader["role"].ToString(),
                                    Username = reader["username"].ToString(),
                                    Password = reader["password"].ToString()
                                };

                                // Add the newly created User object to the employees list
                                employees.Add(employee);
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return employees;
        }
        
        //--------------------------------------------------------------------------------------
        // Method to get the current active shift for a user
        public Shift GetCurrentShift(string userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM employee_timesheet WHERE employee_id = @UserId AND hours_worked IS NULL"; //Looks for the shift with that UserId where hours worked is null
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //If a shift exists, return it
                            return new Shift
                            {
                                ShiftId = int.Parse(reader["timesheet_id"].ToString()),
                                ClockInTime = DateTime.Parse(reader["shift_start"].ToString()),
                                ClockOutTime = reader["shift_end"] != DBNull.Value ? DateTime.Parse(reader["shift_end"].ToString()) : DateTime.MinValue, //Handle null ClockOutTime
                                HoursWorked = reader["hours_worked"] != DBNull.Value ? decimal.Parse(reader["hours_worked"].ToString()) : 0m
                            };
                        }
                    }
                }
            }

            return null; //No active shift found
        }
        
        
        //returns a list of shifts belonging to the logged-in user
        public List<Shift> GetEmployeeShifts(User user)
        {
            List<Shift> shifts = new List<Shift>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM employee_timesheet WHERE employee_id = @UserId;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", user.UserId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Reading...");
                                
                                Shift shift = new Shift()
                                {
                                    UserId = user.UserId,
                                    ShiftId = int.Parse(reader["timesheet_id"].ToString()),
                                    ShiftDate = DateTime.Parse(reader["date"].ToString()),
                                    ClockInTime= DateTime.Parse(reader["shift_start"].ToString()),
                                    ClockOutTime = DateTime.Parse(reader["shift_end"].ToString()),
                                    HoursWorked = decimal.Parse(reader["hours_worked"].ToString()),
                                };

                                // Add the newly created shift to the list
                                shifts.Add(shift);
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return shifts;
        }
        
        public List<Shift> GetShifts()
        {
            List<Shift> shifts = new List<Shift>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM employee_timesheet;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Reading...");
                                
                                Shift shift = new Shift()
                                {
                                    UserId = reader["employee_id"].ToString(),
                                    ShiftId = int.Parse(reader["timesheet_id"].ToString()),
                                    ShiftDate = DateTime.Parse(reader["date"].ToString()),
                                    ClockInTime= DateTime.Parse(reader["shift_start"].ToString()),
                                    ClockOutTime = DateTime.Parse(reader["shift_end"].ToString()),
                                    HoursWorked = decimal.Parse(reader["hours_worked"].ToString()),
                                };

                                // Add the newly created shift to the list
                                shifts.Add(shift);
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return shifts;
        }
        
    }
}