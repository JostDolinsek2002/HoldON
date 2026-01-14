using HoldON.Data;
using HoldON.Models;

namespace HoldON.Services;

public class ExerciseService
{
    private readonly AppDatabase _db;

    public ExerciseService(AppDatabase db)
    {
        _db = db;
    }

    public Task<List<Exercise>> GetAllAsync()
        => _db.GetAllAsync<Exercise>();

    public async Task<List<Exercise>> SearchAsync(string? text, string? group)
    {
        var all = await GetAllAsync();

        if (!string.IsNullOrWhiteSpace(group) && group != "All")
            all = all.Where(e => e.MuscleGroup.Equals(group, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrWhiteSpace(text))
            all = all.Where(e => e.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();

        return all;
    }

    public async Task AddAsync(Exercise exercise)
    {
        if (string.IsNullOrWhiteSpace(exercise.Name))
            throw new ArgumentException("Ime vaje je obvezno");

        if (string.IsNullOrWhiteSpace(exercise.MuscleGroup))
            throw new ArgumentException("Mišična skupina je obvezna");

        await _db.InsertAsync(exercise);
    }

    public Task<Exercise?> GetByIdAsync(int id)
        => _db.GetByIdAsync<Exercise>(id);
}
