using BadgerClan.MAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BadgerClan.MAUI.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string activeMode;
    private readonly IApiService apiService;

    public MainPageViewModel(IApiService apiService)
    {
        ActiveMode = "Do Nothing";
        this.apiService = apiService;
    }
    public string ApiUri { get => apiService.GetClientUri();}

    [RelayCommand]
    public async Task ActivateRunAndGun()
    {
        ActiveMode = await apiService.ActivateRunAndGun();
    }
    [RelayCommand]
    public async Task ActivateDoNothing()
    {
        ActiveMode = await apiService.ActivateDoNothing();
    }
    [RelayCommand]
    public async Task ActivateTurtle()
    {
        ActiveMode = await apiService.ActivateTurtle();
    }
}
