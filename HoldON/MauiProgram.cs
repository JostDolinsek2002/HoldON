using Microsoft.Extensions.Logging;
using HoldON.Data;
using HoldON.Services;
using HoldON.ViewModels;
using HoldON.Views;

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

            // SQLite database (Singleton)
            builder.Services.AddSingleton<AppDatabase>(sp =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "holdon.db3");
                return new AppDatabase(dbPath);
            });

            // Services
            builder.Services.AddSingleton<ExerciseService>();

            // ViewModels
            builder.Services.AddTransient<ExercisesViewModel>();

            // Views (Pages)
            builder.Services.AddTransient<ExercisesPage>();
            builder.Services.AddTransient<AddExercisePage>();
            builder.Services.AddTransient<ExerciseDetailPage>();
            builder.Services.AddSingleton<TrainingService>();
            builder.Services.AddTransient<StartTrainingViewModel>();
            builder.Services.AddTransient<StartTrainingPage>();


            return builder.Build();
        }
    }
}
