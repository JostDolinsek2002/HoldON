using HoldON.ViewModels;

namespace HoldON.Views;

public partial class ExercisesPage : ContentPage
{
    private readonly ExercisesViewModel _vm;

    public ExercisesPage(ExercisesViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.ReloadAsync();
    }
}
