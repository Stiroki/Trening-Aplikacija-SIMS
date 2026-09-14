using System;
using Avalonia.Controls;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class NotificationsWindow : Window
{
    public NotificationsWindow() : this(Guid.Empty)
    {
    }
    public NotificationsWindow(Guid userId)
    {
        InitializeComponent();

        NotificationsViewModel viewModel = new NotificationsViewModel(userId);
        viewModel.BackRequested += () => Close();

        DataContext = viewModel;
    }
}