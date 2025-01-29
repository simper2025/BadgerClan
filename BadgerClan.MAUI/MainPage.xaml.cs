using BadgerClan.MAUI.Services;
using BadgerClan.MAUI.ViewModels;

namespace BadgerClan.MAUI
{
    public partial class MainPage : ContentPage
    {

        public MainPage(IApiService apiService)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(apiService);
        }
    }

}
