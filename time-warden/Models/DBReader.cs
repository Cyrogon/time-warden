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
    }
}