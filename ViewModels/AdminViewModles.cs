using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class AdminViewModel : ViewModelBase
{
    private readonly AdminService _adminService;

    [ObservableProperty]
    private string _welcomeMessage;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public ObservableCollection<Trainer> PendingTrainers { get; } = new();
    public ObservableCollection<Client> PendingClients { get; } = new();
    public ObservableCollection<Payment> OverduePayments { get; } = new();
    public ObservableCollection<Payment> CommissionPayments { get; } = new();
    public ObservableCollection<Trainer> AllTrainersByRating { get; } = new();

    public AdminViewModel(Admin admin) : this(admin, new AdminService())
    {
    }

    public AdminViewModel(Admin admin, AdminService adminService)
    {
        _welcomeMessage = $"Dobrodošli, {admin.Name}";
        _adminService = adminService;

        LoadPendingTrainers();
        LoadPendingClients();
        LoadOverduePayments();
        LoadCommissionPayments();
        LoadTrainersByRating();
    }

    private void LoadPendingTrainers()
    {
        PendingTrainers.Clear();
        foreach (var trainer in _adminService.GetPendingTrainerRegistrations())
        {
            PendingTrainers.Add(trainer);
        }
    }

    private void LoadPendingClients()
    {
        PendingClients.Clear();
        foreach (var client in _adminService.GetPendingClientRegistrations())
        {
            PendingClients.Add(client);
        }
    }

    private void LoadOverduePayments()
    {
        OverduePayments.Clear();
        foreach (var payment in _adminService.GetOverduePayments())
        {
            OverduePayments.Add(payment);
        }
    }

    private void LoadCommissionPayments()
    {
        CommissionPayments.Clear();
        foreach (var payment in _adminService.GetCommissionPayments())
        {
            CommissionPayments.Add(payment);
        }
    }

    private void LoadTrainersByRating()
    {
        AllTrainersByRating.Clear();
        foreach (var trainer in _adminService.GetTrainersSortedByRating())
        {
            AllTrainersByRating.Add(trainer);
        }
    }

    public string GetTrainerName(Guid trainerId) => _adminService.GetTrainerName(trainerId);

    [RelayCommand]
    private void ApproveTrainer(Trainer trainer)
    {
        if (trainer == null) return;

        _adminService.VerifyTrainer(trainer.Id);
        PendingTrainers.Remove(trainer);
        StatusMessage = $"{trainer.Name} {trainer.LastName} je odobren.";
    }

    [RelayCommand]
    private void RejectTrainer(Trainer trainer)
    {
        if (trainer == null) return;

        _adminService.RejectTrainer(trainer.Id, string.Empty);
        PendingTrainers.Remove(trainer);
        StatusMessage = $"{trainer.Name} {trainer.LastName} je odbijen.";
    }

    [RelayCommand]
    private void ApproveClient(Client client)
    {
        if (client == null) return;

        _adminService.VerifyClient(client.Id);
        PendingClients.Remove(client);
        StatusMessage = $"{client.Name} {client.LastName} je odobren.";
    }

    [RelayCommand]
    private void RejectClient(Client client)
    {
        if (client == null) return;

        _adminService.RejectClient(client.Id, string.Empty);
        PendingClients.Remove(client);
        StatusMessage = $"{client.Name} {client.LastName} je odbijen.";
    }

    [RelayCommand]
    private void MarkPaymentAsPaid(Payment payment)
    {
        if (payment == null) return;

        _adminService.MarkPaymentAsPaid(payment.Id);
        OverduePayments.Remove(payment);
        CommissionPayments.Remove(payment);
        StatusMessage = $"Uplata za period {payment.Period} označena kao plaćena.";
    }

    [RelayCommand]
    private void WarnTrainer(Trainer trainer)
    {
        if (trainer == null) return;

        _adminService.WarnTrainer(trainer.Id, string.Empty);
        trainer.WarningCount += 1;
        StatusMessage = $"{trainer.Name} {trainer.LastName} je upozoren ({trainer.WarningCount}. put).";
    }

    [RelayCommand]
    private void RemoveTrainer(Trainer trainer)
    {
        if (trainer == null) return;

        _adminService.RemoveTrainer(trainer.Id);
        AllTrainersByRating.Remove(trainer);
        StatusMessage = $"{trainer.Name} {trainer.LastName} je uklonjen sa platforme.";
    }
}