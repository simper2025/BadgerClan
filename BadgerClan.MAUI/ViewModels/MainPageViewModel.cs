using BadgerClan.MAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BadgerClan.MAUI.ViewModels;

public partial class MainPageViewModel(IApiService apiService) : ObservableObject
{
    [ObservableProperty]
    private string activeMode;

    public string ApiUri { get => apiService.GetClientUri();}

    [RelayCommand]
    public async void ActivateRunAndGun()
    {
        ActiveMode = await apiService.ActivateRunAndGun();
    }
}
