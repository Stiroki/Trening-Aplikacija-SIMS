using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public class TrainingItemDisplay
{
    public TrainingItem Item { get; }
    public Exercise Exercise { get; }

    public string SetsRepsText => $"{Item.Sets} x {Item.Reps}";
    public string RatingText => Item.Rating.HasValue ? $"Ocena : {Item.Rating}/5" : "Neocenjeno";
    public bool HasComment => !string.IsNullOrEmpty(Item.ClientComment);

    public TrainingItemDisplay(TrainingItem item, Exercise exercise)
    {
        Item = item;
        Exercise = exercise;
    }
}

public partial class TrainingSessionDisplay : ObservableObject
{
    public TrainingSession Session { get; }
    public string DateText => Session.DateCreated.ToString("dd.MM.yyyy");
    public string StatusText => Session.IsCompleted ? "Završen" : "Aktivan";
    public string OverallRatingText => Session.OverallRating.HasValue ? $"Ocena: {Session.OverallRating}/5" : "";
    public bool HasOverallComment => !string.IsNullOrEmpty(Session.OverallComment);

    public ObservableCollection<TrainingItemDisplay> Items { get; } = new();

    [ObservableProperty] 
    private bool _isExpanded;

    public TrainingSessionDisplay(TrainingSession session)
    {
        Session = session;
    }

    [RelayCommand]
    private void ToggleExpand()
    {
        IsExpanded = !IsExpanded;
    }
}

public partial class MyTrainingsViewModel : ViewModelBase
{
    private readonly TrainingService _trainingService;
    private readonly Client _client;
    private readonly Dictionary<Guid, Exercise> _exerciseCache = new();

    public ObservableCollection<TrainingSessionDisplay> Sessions { get; } = new();
    public event Action? BackRequested;
    public event Action<TrainingSession> RateRequested;

    [ObservableProperty]
    private bool _hasNoSessions;
    
    public MyTrainingsViewModel(Client client) : this(client, new TrainingService())
    {
    }

    public MyTrainingsViewModel(Client client, TrainingService trainingService)
    {
        _client = client;
        _trainingService = trainingService;
        LoadSessions();
    }

    private void LoadSessions()
    {
        Sessions.Clear();

        List<TrainingSession> sessions = _trainingService.GetClientSessions(_client.Id).
            OrderByDescending(s => s.DateCreated).ToList();

        HasNoSessions = sessions.Count == 0;
        foreach (TrainingSession session in sessions)
        {
            TrainingSessionDisplay display = new TrainingSessionDisplay(session);
            foreach (TrainingItem item in session.Items)
            {
                Exercise exercise = GetExercise(item.ExerciseId)
                                    ?? new Exercise { Name = "Nepoznata vežba", Description = "" };
                display.Items.Add(new TrainingItemDisplay(item, exercise));
            }
            Sessions.Add(display);
        }
    }

    private Exercise? GetExercise(Guid exerciseId)
    {
        if (_exerciseCache.TryGetValue(exerciseId, out var cached))
            return cached;

        var exercise = _trainingService.GetExerciseById(exerciseId);
        if (exercise != null)
        {
            _exerciseCache[exerciseId] = exercise;
        }

        return exercise;
    }

    [RelayCommand]
    private void RateTraining(TrainingSessionDisplay item)
    {
        if (item == null || item.Session.IsCompleted) return;
        RateRequested?.Invoke(item.Session);
    }

    public void RefreshSessions()
    {
        LoadSessions();
    }

    [RelayCommand]
    private void Back()
    {
        BackRequested?.Invoke();
    }
}