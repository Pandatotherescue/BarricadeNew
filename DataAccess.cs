using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BarricadeNew.Helpers;

namespace BarricadeNew
{
    public static class DataAccess
    {
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BarricadeNew",
            "data.db"
        );

        // Ensure the database and folder exist
        public static void InitializeDatabase()
        {
            string folderPath = Path.GetDirectoryName(DbPath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath!);
            }

            if (!File.Exists(DbPath))
            {
                using var connection = new SqliteConnection($"Data Source={DbPath}");
                connection.Open();

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

        // Add a new credential
        public static void AddCredential(string service, string username, string password)
        {
            var masterKey = MasterKeyManager.GetMasterKey();
            var encryptedPassword = EncryptionHelper.Encrypt(password, masterKey);

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Credentials (Service, Username, Password)
                VALUES ($service, $username, $password);
            ";
            command.Parameters.AddWithValue("$service", service);
            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$password", encryptedPassword);

            command.ExecuteNonQuery();
        }

        // Retrieve all credentials
        public static List<(int Id, string Service, string Username, string Password)> GetAllCredentials()
        {
            var credentials = new List<(int, string, string, string)>();
            var masterKey = MasterKeyManager.GetMasterKey();

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Service, Username, Password FROM Credentials;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var service = reader.GetString(1);
                var username = reader.GetString(2);
                var encryptedPassword = reader.GetString(3);

                try
                {
                    // Log encrypted password for debugging
                    Console.WriteLine($"Dekrypterer password for {service}: {encryptedPassword}");

                    // Attempt decryption
                    var password = EncryptionHelper.Decrypt(encryptedPassword, masterKey);

                    // Add to the result
                    credentials.Add((id, service, username, password));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fejl under dekryptering: {ex.Message}");
                    Console.WriteLine($"Problem med password for {service}: {encryptedPassword}");
                }
            }

            return credentials;
        }

        // Delete a credential by ID
        public static void DeleteCredential(int id)
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Credentials WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }

        // Get the database file path
        public static string GetDatabasePath()
        {
            return DbPath;
        }
    }
}
