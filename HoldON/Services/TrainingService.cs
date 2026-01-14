using HoldON.Data;
using HoldON.Models;

namespace HoldON.Services;

public class TrainingService
{
    private readonly AppDatabase _db;

    public TrainingService(AppDatabase db)
    {
        _db = db;
    }

    public Task<List<Exercise>> GetExercisesAsync()
        => _db.GetAllAsync<Exercise>();

    public async Task<TrainingSession> CreateNewSessionAsync()
    {
        var user = await _db.GetOrCreateDefaultUserAsync();

        var session = new TrainingSession
        {
            UserId = user.UserId,
            Date = DateTime.Today,
            Type = "Workout",
            Location = "",
            Notes = ""
        };

        await _db.InsertAsync(session);
        // SQLite-net običajno nastavi AutoIncrement PK nazaj na objekt
        return session;
    }

    public async Task AddSetAsync(int trainingId, int exerciseId, int reps, float weight, int restSeconds)
    {
        if (trainingId <= 0) throw new ArgumentException("TrainingId ni veljaven.");
        if (exerciseId <= 0) throw new ArgumentException("Izberi vajo.");
        if (reps <= 0) throw new ArgumentException("Reps mora biti > 0.");
        if (weight < 0) throw new ArgumentException("Weight ne sme biti negativen.");
        if (restSeconds < 0) restSeconds = 0;

        // določi set_order = zadnji + 1
        var existing = await _db.GetSetsForTrainingAsync(trainingId);
        var nextOrder = (existing.Count == 0) ? 1 : existing.Max(s => s.SetOrder) + 1;

        var set = new SetEntry
        {
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            SetOrder = nextOrder,
            Reps = reps,
            Weight = weight,
            RestSeconds = restSeconds
        };

        await _db.InsertAsync(set);
    }

    public Task<List<SetEntry>> GetSetsAsync(int trainingId)
        => _db.GetSetsForTrainingAsync(trainingId);

    public Task<Exercise?> GetExerciseByIdAsync(int id)
        => _db.GetByIdAsync<Exercise>(id);
}
