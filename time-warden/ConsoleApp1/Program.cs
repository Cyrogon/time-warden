using time_warden.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

class Program
{
    static void Main(string[] args)
    {
        // Basic Console Output
        Console.WriteLine("Build Successful, testing DB Read...");

        // Create an instance of DBReader
        DBReader db = new DBReader();
        
        // Call the ReadData method
        db.ReadData();

        // Pause the console to keep it open
        Console.ReadKey();
    }
}