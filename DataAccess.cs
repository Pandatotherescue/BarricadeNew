using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;

namespace BarricadeNew;


    public static class DataAccess
    {
        // Database path in AppData
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BarricadeNew",
            "data.db"
        );

        // Ensure the database and folder exist
        public static void InitializeDatabase()
        {
            // Create directory if it doesn't exist
            string folderPath = Path.GetDirectoryName(DbPath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath!);
            }

            // Create database file if it doesn't exist
            if (!File.Exists(DbPath))
            {
                using var connection = new SqliteConnection($"Data Source={DbPath}");
                connection.Open();

                //Create a table for storing credentials
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Credentials (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Service TEXT NOT NULL,
                        Username TEXT NOT NULL,
                        Password TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
            }
        }

        // Example method to retrieve the database path (optional)
        public static string GetDatabasePath() => DbPath;
    }
    
