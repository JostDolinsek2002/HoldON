using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HoldON.Models;
using HoldON.Services;

namespace HoldON.ViewModels;

public class SetEntryDisplay
{
    public int SetOrder { get; set; }
    public string ExerciseName { get; set; } = "";
    public int Reps { get; set; }
    public float Weight { get; set; }
    public int RestSeconds { get; set; }
}

public class StartTrainingViewModel : INotifyPropertyChanged
{
    private readonly TrainingService _service;

    public ObservableCollection<Exercise> Exercises { get; } = new();
    public ObservableCollection<SetEntryDisplay> Sets { get; } = new();

    private TrainingSession? _session;
    public int TrainingId => _session?.TrainingId ?? 0;

    private Exercise? _selectedExercise;
    public Exercise? SelectedExercise
    {
        get => _selectedExercise;
        set { _selectedExercise = value; OnPropertyChanged(); }
    }

    private string _repsText = "";
    public string RepsText
    {
        get => _repsText;
        set { _repsText = value; OnPropertyChanged(); }
    }

    private string _weightText = "";
    public string WeightText
    {
        get => _weightText;
        set { _weightText = value; OnPropertyChanged(); }
    }

    private string _restText = "";
    public string RestText
    {
        get => _restText;
        set { _restText = value; OnPropertyChanged(); }
    }

    public ICommand AddSetCommand { get; }
    public ICommand NewSessionCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public StartTrainingViewModel(TrainingService service)
    {
        _service = service;

        AddSetCommand = new Command(async () => await AddSetAsync());
        NewSessionCommand = new Command(async () => await StartNewSessionAsync());
    }

    public async Task InitAsync()
    {
        // 1) naloži vaje
        var ex = await _service.GetExercisesAsync();
        Exercises.Clear();
        foreach (var e in ex.OrderBy(x => x.MuscleGroup).ThenBy(x => x.Name))
            Exercises.Add(e);

        // 2) začni trening, če ga še ni
        if (_session == null)
            await StartNewSessionAsync();
    }

    public async Task StartNewSessionAsync()
    {
        _session = await _service.CreateNewSessionAsync();
        OnPropertyChanged(nameof(TrainingId));

        // reset UI
        SelectedExercise = null;
        RepsText = "";
        WeightText = "";
        RestText = "";

        await ReloadSetsAsync();
    }

    private async Task AddSetAsync()
    {
        if (_session == null)
            await StartNewSessionAsync();

        if (SelectedExercise == null)
            throw new Exception("Izberi vajo.");

        if (!int.TryParse(RepsText, out var reps) || reps <= 0)
            throw new Exception("Vnesi veljavne ponovitve (reps).");

        if (!float.TryParse(WeightText, out var weight) || weight < 0)
            throw new Exception("Vnesi veljavno težo (weight).");

        int rest = 0;
        if (!string.IsNullOrWhiteSpace(RestText))
        {
            if (!int.TryParse(RestText, out rest) || rest < 0)
                throw new Exception("Rest mora biti število (sekunde).");
        }

        await _service.AddSetAsync(_session!.TrainingId, SelectedExercise.ExerciseId, reps, weight, rest);

        // počisti inpute za hitro vnašanje
        RepsText = "";
        WeightText = "";
        RestText = "";

        await ReloadSetsAsync();
    }

    private async Task ReloadSetsAsync()
    {
        Sets.Clear();

        if (_session == null) return;

        var raw = await _service.GetSetsAsync(_session.TrainingId);

        // map exerciseId -> name (iz že naloženih vaj)
        var nameById = Exercises.ToDictionary(e => e.ExerciseId, e => e.Name);

        foreach (var s in raw.OrderBy(x => x.SetOrder))
        {
            Sets.Add(new SetEntryDisplay
            {
                SetOrder = s.SetOrder,
                ExerciseName = nameById.TryGetValue(s.ExerciseId, out var n) ? n : $"#{s.ExerciseId}",
                Reps = s.Reps,
                Weight = s.Weight,
                RestSeconds = s.RestSeconds
            });
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
