using HoldON.Models;
using HoldON.Services;

namespace HoldON.Views;

public partial class AddExercisePage : ContentPage
{
    private readonly ExerciseService _service;

    public AddExercisePage(ExerciseService service)
    {
        InitializeComponent();
        _service = service;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            var ex = new Exercise
            {
                Name = NameEntry.Text,
                MuscleGroup = GroupEntry.Text,
                Description = DescEditor.Text,
                MediaUrl = MediaEntry.Text
            };

            await _service.AddAsync(ex);
            await DisplayAlert("OK", "Vaja dodana", "Zapri");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Napaka", ex.Message, "OK");
        }
    }
}
