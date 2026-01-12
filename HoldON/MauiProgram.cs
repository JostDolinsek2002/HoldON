using Microsoft.Extensions.Logging;
using HoldON.Data;

namespace HoldON
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<AppDatabase>(sp =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "holdon.db3");
                return new AppDatabase(dbPath);
            });

            return builder.Build();
        }
    }
}
