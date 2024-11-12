using System;

namespace time_warden.Models
{
    public class ConsoleTesting
    {
        static void Main(string[] args)
        {
            // Basic Console Output
            Console.WriteLine("Build Successful, testing DB Read...");

            DBReader db = new DBReader();
            
            db.ReadData();
        }
    }
}