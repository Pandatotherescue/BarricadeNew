using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using BarricadeNew.Models;
using ReactiveUI;

namespace BarricadeNew.ViewModels;

public class HomeViewModel : ViewModelBase
{
    // Collection to bind to the DataGrid
        private ObservableCollection<CredentialViewModel> _credentials;
        public ObservableCollection<CredentialViewModel> Credentials
        {
            get => _credentials;
            set => this.RaiseAndSetIfChanged(ref _credentials, value);
        }
        
        
        public ReactiveCommand<Unit, Unit> GeneratePasswordCommand { get; }

        // Input properties
        private string _service;
        public string Service
        {
            get => _service;
            set => this.RaiseAndSetIfChanged(ref _service, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        // Commands
        public ReactiveCommand<Unit, Unit> AddCredentialCommand { get; }

        // Constructor
        public HomeViewModel()
        {
            // Initialize the collection
            Credentials = new ObservableCollection<CredentialViewModel>();

            // Load existing credentials from database
            LoadCredentials();

            // Define the AddCredentialCommand
            AddCredentialCommand = ReactiveCommand.Create(AddCredential);
            
            GeneratePasswordCommand = ReactiveCommand.Create(() =>
            {
                Password = PasswordGenerator.Generate(16); // Generates a 16-character password
            });
        }

        // Method to add a new credential
        private void AddCredential()
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Service) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
            {
                // Optional: Notify user about missing input
                return;
            }

            // Add to database
            DataAccess.AddCredential(Service, Username, Password);

            // Add to the ObservableCollection
            Credentials.Add(new CredentialViewModel
            {
                Service = Service,
                Username = Username,
                Password = Password
            });

            // Clear input fields
            Service = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }

        // Method to load existing credentials from database
        private void LoadCredentials()
        {
            var credentials = DataAccess.GetAllCredentials();

            foreach (var (service, username, password) in credentials)
            {
                Credentials.Add(new CredentialViewModel
                {
                    Service = service,
                    Username = username,
                    Password = password
                });
            }
        }
    }

    // ViewModel for individual credentials
    public class CredentialViewModel
    {
        public string Service { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    

    
    
    
        
