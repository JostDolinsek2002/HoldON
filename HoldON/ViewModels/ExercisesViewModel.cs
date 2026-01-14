using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HoldON.Models;
using HoldON.Services;

namespace HoldON.ViewModels;

public class ExercisesViewModel : INotifyPropertyChanged
{
    private readonly ExerciseService _service;
    private bool _isReloading;


    public ObservableCollection<Exercise> Exercises { get; } = new();
    public ObservableCollection<string> Groups { get; } = new();

    private string? _searchText;
    public string? SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); _ = ReloadAsync(); }
    }

    private string _selectedGroup = "All";
    public string SelectedGroup
    {
        get => _selectedGroup;
        set { _selectedGroup = value; OnPropertyChanged(); _ = ReloadAsync(); }
    }

    public ICommand OpenAddCommand { get; }
    public ICommand OpenDetailCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ExercisesViewModel(ExerciseService service)
    {
        _service = service;

        OpenAddCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(Views.AddExercisePage)));

        OpenDetailCommand = new Command<Exercise>(async (e) =>
        {
            if (e == null) return;
            await Shell.Current.GoToAsync(
                $"{nameof(Views.ExerciseDetailPage)}?id={e.ExerciseId}");
        });
    }

    public async Task ReloadAsync()
    {
        if (_isReloading) return;

        try
        {
            _isReloading = true;

            var list = await _service.SearchAsync(SearchText, SelectedGroup);

            // 1) Vaje
            Exercises.Clear();
            foreach (var e in list.OrderBy(x => x.MuscleGroup).ThenBy(x => x.Name))
                Exercises.Add(e);

            // 2) Skupine
            var groups = list.Select(x => x.MuscleGroup)
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .Distinct(StringComparer.OrdinalIgnoreCase)
                             .OrderBy(x => x)
                             .ToList();

            Groups.Clear();
            Groups.Add("All");
            foreach (var g in groups)
                Groups.Add(g);

            // 3) Popravi izbrano skupino BREZ sprožitve novega Reload
            if (!Groups.Contains(_selectedGroup))
            {
                _selectedGroup = "All";
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }
        finally
        {
            _isReloading = false;
        }
    }


    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
