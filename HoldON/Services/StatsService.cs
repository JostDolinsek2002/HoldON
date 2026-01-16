using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoldON.Data;
using HoldON.Models;

namespace HoldON.Services;

public class ExerciseTrendRow
{
    public DateTime Date { get; set; }
    public int TrainingId { get; set; }
    public float Volume { get; set; }     // sum(reps * weight)
    public float MaxWeight { get; set; }  // max(weight)
}

public class StatsService
{
    private readonly AppDatabase _db;

    public StatsService(AppDatabase db)
    {
        _db = db;
    }

    public Task<List<Exercise>> GetExercisesAsync()
        => _db.GetAllAsync<Exercise>();

    private async Task<List<SetEntry>> GetSetsForExerciseAsync(int exerciseId)
    {
        var sets = await _db.GetAllAsync<SetEntry>();
        return sets.Where(s => s.ExerciseId == exerciseId).ToList();
    }

    public async Task<float> GetPrAsync(int exerciseId)
    {
        var sets = await GetSetsForExerciseAsync(exerciseId);
        return sets.Select(s => s.Weight)
                   .DefaultIfEmpty(0f)
                   .Max();
    }

    public async Task<float> GetVolumeAsync(int exerciseId)
    {
        var sets = await GetSetsForExerciseAsync(exerciseId);
        return sets.Select(s => s.Reps * s.Weight)
                   .DefaultIfEmpty(0f)
                   .Sum();
    }

    public async Task<List<ExerciseTrendRow>> GetTrendAsync(int exerciseId, int take = 10)
    {
        var sessions = await _db.GetAllAsync<TrainingSession>();
        var sets = await GetSetsForExerciseAsync(exerciseId);

        var trend =
            (from s in sets
             join t in sessions on s.TrainingId equals t.TrainingId
             group s by new { t.TrainingId, Date = t.Date.Date } into g
             select new ExerciseTrendRow
             {
                 TrainingId = g.Key.TrainingId,
                 Date = g.Key.Date,
                 Volume = g.Sum(x => x.Reps * x.Weight),
                 MaxWeight = g.Max(x => x.Weight)
             })
            .OrderByDescending(x => x.Date)
            .Take(take)
            .ToList();

        return trend;
    }

    // dodatek za F5 funkcijo
    public async Task<string> GetStrengthLevelAsync(int exerciseId, float pr, string gender, float? bodyWeight)
    {
        // Load all standards (prototype: in-memory ok)
        var standards = await _db.GetAllAsync<StrengthStandard>();
        System.Diagnostics.Debug.WriteLine($"STANDARDS COUNT: {standards.Count}");


        var filtered = standards
            .Where(s => s.ExerciseId == exerciseId)
            .Where(s => string.Equals(s.Gender, gender, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (bodyWeight.HasValue)
        {
            var bw = bodyWeight.Value;
            filtered = filtered
                .Where(s => bw >= s.BwMin && bw <= s.BwMax)
                .ToList();
        }

        if (filtered.Count == 0)
            return "Ni normativov";

        // Če PR ni pozitiven, tudi ne moremo oceniti
        if (pr <= 0)
            return "—";

        // Najpreprostejši način:
        // izberi najvišji standard, ki ga dosežeš (pr >= weight_value)
        // Če nobenega ne dosežeš, vrni najnižji nivo iz baze.
        var achieved = filtered
            .Where(s => pr >= s.WeightValue)
            .OrderByDescending(s => s.WeightValue)
            .FirstOrDefault();

        if (achieved != null)
            return achieved.Level;

        // Če ne doseže niti prvega standarda, vrni najlažji nivo (najmanjši weight_value)
        return filtered
            .OrderBy(s => s.WeightValue)
            .First()
            .Level;
    }

}
