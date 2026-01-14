using HoldON.Services;

namespace HoldON.Views;

[QueryProperty(nameof(ExerciseId), "id")]
public partial class ExerciseDetailPage : ContentPage
{
    private readonly ExerciseService _service;
    private string? _mediaUrl;

    public string ExerciseId { get; set; } = "";

    public ExerciseDetailPage(ExerciseService service)
    {
        InitializeComponent();
        _service = service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!int.TryParse(ExerciseId, out var id)) return;

        var ex = await _service.GetByIdAsync(id);
        if (ex == null) return;

        NameLbl.Text = ex.Name;
        GroupLbl.Text = ex.MuscleGroup;
        DescLbl.Text = ex.Description;

        _mediaUrl = ex.MediaUrl;
        OpenMediaBtn.IsVisible = !string.IsNullOrWhiteSpace(_mediaUrl);
    }

    private async void OnOpenMediaClicked(object sender, EventArgs e)
    {
        if (_mediaUrl != null)
            await Launcher.OpenAsync(_mediaUrl);
    }
}
