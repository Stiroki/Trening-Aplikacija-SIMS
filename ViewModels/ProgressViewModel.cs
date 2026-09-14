using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class ProgressViewModel : ViewModelBase
{
    private readonly ClientService _clientService;
    private readonly Guid _clientId;

    public ObservableCollection<ProgressEntry> History { get; } = new();

    [ObservableProperty] 
    private double _weight;
    
    [ObservableProperty] 
    private double _height;
    
    [ObservableProperty] 
    private double _chest;
    
    [ObservableProperty] 
    private double _waist;
    
    [ObservableProperty] 
    private double _hips;
    
    [ObservableProperty] 
    private double _biceps;
    
    [ObservableProperty] 
    private double _thigh;
    
    [ObservableProperty] 
    private string _comment = "";
    
    [ObservableProperty] 
    private string _statusMessage = "";
    
    [ObservableProperty] 
    private bool _isStatusVisible;
    
    [ObservableProperty] 
    private bool _hasNoEntries;

    public event Action? BackRequested;
    
    public ProgressViewModel(Guid clientId) : this(clientId, new ClientService())
    {
    }

    public ProgressViewModel(Guid clientId, ClientService clientService)
    {
        _clientId = clientId;
        _clientService = clientService;

        LoadHistory();
    }

    private void LoadHistory()
    {
        History.Clear();
        List<ProgressEntry> entries = _clientService.GetProgressHistory(_clientId);
        HasNoEntries = entries.Count == 0;

        foreach (var entry in entries)
        {
            History.Add(entry);
        }
    }

    [RelayCommand]
    private void AddEntry()
    {
        if (Weight <= 0)
        {
            StatusMessage = "Unesite bar težinu.";
            IsStatusVisible = true;
            return;
        }

        try
        {
            ProgressEntry entry = new ProgressEntry
            {
                ClientId = _clientId,
                Weight = Weight,
                Chest = Chest,
                Biceps = Biceps,
                Hips = Hips,
                Waist = Waist,
                Thigh = Thigh,
                Comment = Comment
            };
            _clientService.AddProgressEntry(entry);

            Weight = 0;
            Chest = 0;
            Waist = 0;
            Hips = 0;
            Biceps = 0;
            Thigh = 0;
            Comment = "";
            StatusMessage = "Merenje sačuvano.";
            _isStatusVisible = true;
            LoadHistory();
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