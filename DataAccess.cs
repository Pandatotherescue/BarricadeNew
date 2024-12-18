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

        // Insert a new credential into the database
        public static void AddCredential(string service, string username, string password)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Credentials (Service, Username, Password)
                VALUES (@service, @username, @password);
            ";
            command.Parameters.AddWithValue("@service", service);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            command.ExecuteNonQuery();
        }

        // Retrieve all credentials from the database
        public static List<(string Service, string Username, string Password)> GetAllCredentials()
        {
            List<(string Service, string Username, string Password)> credentials = new();

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Service, Username, Password FROM Credentials;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string service = reader.GetString(0);
                string username = reader.GetString(1);
                string password = reader.GetString(2);

                credentials.Add((service, username, password));
            }

            return credentials;
        }
    }
    
