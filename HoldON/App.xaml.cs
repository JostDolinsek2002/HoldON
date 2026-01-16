using HoldON.Data;

namespace HoldON;

public partial class App : Application
{
    public App(AppDatabase db)
    {
        InitializeComponent();

        MainPage = new AppShell();

        _ = db.SeedIfEmptyAsync();
        _ = db.SeedDemoStatsIfEmptyAsync();
        _ = db.SeedStrengthStandardsIfEmptyAsync();

    }
}