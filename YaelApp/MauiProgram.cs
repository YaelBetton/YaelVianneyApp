using Microsoft.Extensions.Logging;
using YaelApp.Pages;
using YaelApp.Services;
using YaelApp.ViewModels;
namespace YaelApp;
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
        // Registration of Services and Pages
        builder.Services.AddSingleton<SportsService>();
        builder.Services.AddTransient<ScoreViewModel>();
        builder.Services.AddTransient<ScorePage>();
        builder.Services.AddTransient<ScoreDetailViewModel>();
        builder.Services.AddTransient<ScoreDetailPage>();
        builder.Services.AddTransient<TennisPage>();
        builder.Services.AddSingleton<VolleyScoreViewModel>();
        builder.Services.AddTransient<VolleyPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
