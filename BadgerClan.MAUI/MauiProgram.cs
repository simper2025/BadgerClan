using BadgerClan.MAUI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BadgerClan.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            var env = "http://localhost:1000";

            builder.Services.AddSingleton<IApiService, ApiService>();
            builder.Services.AddHttpClient("ControllerApi", (o) =>
            {
                o.BaseAddress = new Uri(env);
            });


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
