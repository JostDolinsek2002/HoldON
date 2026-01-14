using HoldON.ViewModels;

namespace HoldON.Views;

public partial class StartTrainingPage : ContentPage
{
    private readonly StartTrainingViewModel _vm;

    public StartTrainingPage(StartTrainingViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _vm.InitAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Napaka", ex.Message, "OK");
        }
    }
}
