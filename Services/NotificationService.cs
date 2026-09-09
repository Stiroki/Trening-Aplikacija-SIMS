using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using TreningAplikacija.Models;
using TreningAplikacija.Repositories;
using Notification = TreningAplikacija.Models.Notification;

namespace TreningAplikacija.Services;

public class NotificationService
{
    private readonly JsonRepository<Notification> _notificationRepo;

    public NotificationService()
    {
        _notificationRepo = new JsonRepository<Notification>("notifications.json");
    }

    public void CreateNotification(Guid userId, NotificationType type, string message)
    {
        Notification notification = new Notification
        {
            UserId = userId,
            Type = type,
            Message = message
        };
        
        _notificationRepo.Create(notification);
    }

    public List<Notification> GetNotifications(Guid userId)
    {
        return _notificationRepo.GetAll()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.Date)
            .ToList();
    }

    public int GetUnreadCount(Guid userId)
    {
        return _notificationRepo.GetAll()
            .Count(n => n.UserId == userId && !n.IsRead);
    }

    public void MarkAsRead(Guid notificationId)
    {
        Notification notification = _notificationRepo.GetById(notificationId);
        if (notification == null) return;

        notification.IsRead = true;
        _notificationRepo.Update(notification);
    }

    public void MarkAllAsRead(Guid userId)
    {
        List<Notification> unreadNotifications = _notificationRepo.GetAll()
            .Where(n => n.UserId == userId && !n.IsRead).ToList();
        
        foreach (Notification n in unreadNotifications)
        {
            n.IsRead = true;
            _notificationRepo.Update(n);
        }
    }
}