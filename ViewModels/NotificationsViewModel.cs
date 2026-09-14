using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class NotificationsViewModel : ViewModelBase
{
    private readonly NotificationService _notificationService;
    private readonly Guid _userId;

    public ObservableCollection<Notification> Notifications { get; } = new();

    [ObservableProperty] 
    private bool _hasNoNotifications;

    public event Action? BackRequested;
    
    public NotificationsViewModel(Guid userId) : this(userId, new NotificationService())
    {
    }

    public NotificationsViewModel(Guid userId, NotificationService notificationService)
    {
        _userId = userId;
        _notificationService = notificationService;

        LoadNotifications();
    }

    private void LoadNotifications()
    {
        Notifications.Clear();

        List<Notification> notifications = _notificationService.GetNotifications(_userId);
        HasNoNotifications = notifications.Count == 0;

        foreach (var n in notifications)
        {
            Notifications.Add(n);
        }
    }

    [RelayCommand]
    private void MarkAllAsRead()
    {
        _notificationService.MarkAllAsRead(_userId);
        LoadNotifications();
    }

    [RelayCommand]
    private void Back()
    {
        BackRequested?.Invoke();
    }
}