using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HoldON.Models;
using HoldON.Services;

namespace HoldON.ViewModels;

public class TrendDisplayRow
{
    public string DateText { get; set; } = "";
    public string VolumeText { get; set; } = "";
    public string MaxText { get; set; } = "";
}

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly StatsService _service;

    public ObservableCollection<Exercise> Exercises { get; } = new();
    public ObservableCollection<TrendDisplayRow> Trend { get; } = new();

    private Exercise? _selectedExercise;
    public Exercise? SelectedExercise
    {
        get => _selectedExercise;
        set
        {
            _selectedExercise = value;
            OnPropertyChanged();
        }
    }

    private string _prText = "—";
    public string PrText
    {
        get => _prText;
        set { _prText = value; OnPropertyChanged(); }
    }

    private string _volumeText = "—";
    public string VolumeText
    {
        get => _volumeText;
        set { _volumeText = value; OnPropertyChanged(); }
    }

    public ICommand RefreshCommand { get; }

    private bool _isLoading;

    public event PropertyChangedEventHandler? PropertyChanged;

    public StatisticsViewModel(StatsService service)
    {
        _service = service;
        RefreshCommand = new Command(async () => await ReloadAsync());
    }

    public async Task InitAsync()
    {
        if (Exercises.Count == 0)
        {
            var list = await _service.GetExercisesAsync();
            Exercises.Clear();

            foreach (var e in list.OrderBy(x => x.MuscleGroup).ThenBy(x => x.Name))
                Exercises.Add(e);
        }

        if (SelectedExercise == null && Exercises.Count > 0)
            SelectedExercise = Exercises[0];

        await ReloadAsync();
    }

    public async Task ReloadAsync()
    {
        if (_isLoading) return;

        try
        {
            _isLoading = true;

            Trend.Clear();
            PrText = "—";
            VolumeText = "—";
            LevelText = "—";

            if (SelectedExercise == null)
                return;

            var id = SelectedExercise.ExerciseId;

            var pr = await _service.GetPrAsync(id);
            var vol = await _service.GetVolumeAsync(id);
            var trend = await _service.GetTrendAsync(id);

            PrText = pr > 0 ? $"{pr:0.#} kg" : "—";
            VolumeText = vol > 0 ? $"{vol:0.#}" : "—";

            // F5 - normativi
            float? bw = null;
            if (float.TryParse(BodyWeightText, out var bwParsed) && bwParsed > 0)
                bw = bwParsed;

            var level = await _service.GetStrengthLevelAsync(id, pr, SelectedGender, bw);
            LevelText = level;

            foreach (var r in trend)
            {
                Trend.Add(new TrendDisplayRow
                {
                    DateText = r.Date.ToString("dd.MM.yyyy"),
                    VolumeText = $"{r.Volume:0.#}",
                    MaxText = $"{r.MaxWeight:0.#} kg"
                });
            }
        }
        catch
        {
            PrText = "—";
            VolumeText = "—";
            LevelText = "—";
            Trend.Clear();
        }
        finally
        {
            _isLoading = false;
        }
    }

    // F5 funkcija
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public ObservableCollection<string> GenderOptions { get; } = new() { "M", "F" };

    private string _selectedGender = "M";
    public string SelectedGender
    {
        get => _selectedGender;
        set
        {
            _selectedGender = value;
            OnPropertyChanged();
            _ = ReloadAsync();
        }
    }

    private string _bodyWeightText = "";
    public string BodyWeightText
    {
        get => _bodyWeightText;
        set
        {
            _bodyWeightText = value;
            OnPropertyChanged();
            _ = ReloadAsync();
        }
    }

    private string _levelText = "—";
    public string LevelText
    {
        get => _levelText;
        set { _levelText = value; OnPropertyChanged(); }
    }
}
