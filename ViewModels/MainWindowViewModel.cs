using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace BarricadeNew.ViewModels;


public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase? _currentView;
    public ViewModelBase? CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public ReactiveCommand<Unit, ViewModelBase> GoToHomeCommand { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToGenerateCommand { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToVaultCommand { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToSettingsCommand { get; }
    
    public ReactiveCommand<Unit, ViewModelBase> GoToLoginCommand { get; }
    
    public ReactiveCommand<Unit, ViewModelBase> GoToRegisterCommand { get; }
    
    

    private readonly HomeViewModel _homeViewModel = new();
    private readonly GenerateViewModel _generateViewModel = new();
    private readonly VaultViewModel _vaultViewModel = new();
    private readonly SettingsViewModel _settingsViewModel = new();
    private readonly LoginViewModel _loginViewModel = new();
    private readonly RegisterViewModel _registerViewModel = new();

    public MainWindowViewModel()
    {
        
        // Set initial view
        CurrentView = _homeViewModel;

        // Define commands
        GoToHomeCommand = ReactiveCommand.Create<ViewModelBase>(() => CurrentView = _homeViewModel);
        GoToGenerateCommand = ReactiveCommand.Create<ViewModelBase>(() => CurrentView = _generateViewModel);
        GoToVaultCommand = ReactiveCommand.Create<ViewModelBase>(() => CurrentView = _vaultViewModel);
        GoToSettingsCommand = ReactiveCommand.Create<ViewModelBase>(() => CurrentView = _settingsViewModel);
        GoToLoginCommand = ReactiveCommand.Create<ViewModelBase>(() => CurrentView = _loginViewModel);
        GoToRegisterCommand = ReactiveCommand.Create<ViewModelBase>(() => CurrentView = _registerViewModel);
    }
    
}