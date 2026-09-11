using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class RateExerciseItem : ObservableObject
{
    public TrainingItem Item { get; }
    public Exercise Exercise { get; }

    [ObservableProperty] 
    private decimal? _rating;
    
    [ObservableProperty] 
    private string _comment = "";

    public RateExerciseItem(TrainingItem item, Exercise exercise)
    {
        Item = item;
        Exercise = exercise;

        Rating = item.Rating.HasValue ? (decimal)item.Rating.Value : null;
        Comment = item.ClientComment ?? "";
    }
}

public partial class RateTrainingViewModel : ViewModelBase
{
    private readonly TrainingService _trainingService;
    private readonly TrainingSession _session;

    public ObservableCollection<RateExerciseItem> Exercises { get; } = new();

    public string HeaderText => $"Trening - {_session.DateCreated:dd.Mm.yyyy}";

    [ObservableProperty]
    private decimal _overallRating = 5;
    
    [ObservableProperty]
    private string _overallComment = string.Empty;
    
    [ObservableProperty]
    private string _statusMessage = string.Empty;
    
    [ObservableProperty]
    private bool _isStatusVisible;

    public event Action? Completed;
    public event Action? BackRequested;
    
    public RateTrainingViewModel(TrainingSession session)
        : this(session, new TrainingService())
    {
    }

    public RateTrainingViewModel(TrainingSession session, TrainingService trainingService)
    {
        _session = session;
        _trainingService = trainingService;
        LoadExercises();
    }

    private void LoadExercises()
    {
        foreach (TrainingItem item in _session.Items)
        {
            Exercise exercise = _trainingService.GetExerciseById(item.ExerciseId)
                                ?? new Exercise { Name = "Nepoznata vežba", Description = "" };
            Exercises.Add(new RateExerciseItem(item, exercise));
        }
    }

    [RelayCommand]
    private void CompleteTraining()
    {
        if (string.IsNullOrWhiteSpace(OverallComment))
        {
            StatusMessage = "Komentar za trening je obavezan.";
            IsStatusVisible = true;
            return;
        }

        if (OverallRating < 1 || OverallRating > 5)
        {
            StatusMessage = "Ocena mora biti izmedju 1 i 5.";
            IsStatusVisible = true;
            return;
        }

        try
        {
            foreach (var exercise in Exercises)
            {
                if (exercise.Rating.HasValue)
                {
                    _trainingService.RateExercise(_session.Id, exercise.Item.ExerciseId, (int)exercise.Rating.Value,
                        exercise.Comment);
                }
            }

            _trainingService.CompleteAndRateSession(_session.Id, (int)OverallRating, OverallComment);
            Completed?.Invoke();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            IsStatusVisible = true;
        }
    }

    [RelayCommand]
    private void Back()
    {
        BackRequested?.Invoke();
    }
}