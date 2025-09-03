using System.Windows;
using ColourMemory.ViewModels;
using ColourMemory.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ColourMemory;

public partial class App
{
    private IServiceProvider _serviceProvider = null!;
    
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        RegisterServices();
        RunApplication();
    }
    
    private void RegisterServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<IMainViewModel, MainViewModel>();
        services.AddSingleton<MainView>();
        _serviceProvider = services.BuildServiceProvider();
    }
    
    private void RunApplication()
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainView>();
        var mainViewModel = _serviceProvider.GetRequiredService<IMainViewModel>();
        mainWindow.DataContext = mainViewModel;
        mainWindow.Show();
    }
}