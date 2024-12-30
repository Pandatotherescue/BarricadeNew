using ReactiveUI;
using System;
using System.Reactive;
using BarricadeNew.Helpers;

namespace BarricadeNew.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = ReactiveCommand.Create(ExecuteLogin);
            RegisterCommand = ReactiveCommand.Create(ExecuteRegister);
        }

        private void ExecuteLogin()
        {
            try
            {
                var storedMasterKey = MasterKeyManager.GetMasterKey();
                if (ValidateCredentials(storedMasterKey))
                {
                    // Navigate to main app window
                    Console.WriteLine("Login successful!");
                }
                else
                {
                    Console.WriteLine("Invalid login credentials.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
            }
        }
        
        //Metode til registrering
        private void ExecuteRegister()
        {
            //Skal implementere viewchange ved klik på knap, samt tilføje login credentials til DB.
            Console.WriteLine("Register successful!");
        }

        private bool ValidateCredentials(byte[] storedMasterKey)
        {
            // Example validation: check if password matches a derived master key
            var enteredKey = EncryptionHelper.Encrypt(Password, storedMasterKey);
            return Convert.ToBase64String(storedMasterKey) == enteredKey;
        }
    }
}
