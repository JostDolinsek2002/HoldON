using HoldON.Views;


namespace HoldON;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registracija routov (navigacija)
        Routing.RegisterRoute(nameof(StartTrainingPage), typeof(StartTrainingPage));
        Routing.RegisterRoute(nameof(ExercisesPage), typeof(ExercisesPage));
        Routing.RegisterRoute(nameof(StatisticsPage), typeof(StatisticsPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }
}
