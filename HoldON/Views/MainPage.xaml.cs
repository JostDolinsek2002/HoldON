namespace HoldON.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnGoExercisesClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(ExercisesPage));

    private async void OnGoStatsClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(StatisticsPage));

    private async void OnGoStartTrainingClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(StartTrainingPage));
}