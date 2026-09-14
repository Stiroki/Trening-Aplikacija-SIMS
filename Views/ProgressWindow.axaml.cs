using System;
using Avalonia.Controls;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class ProgressWindow : Window
{
    public ProgressWindow() : this(Guid.Empty)
    {
    }
    
    public ProgressWindow(Guid clientId)
    {
        InitializeComponent();

        ProgressViewModel viewModel = new ProgressViewModel(clientId);
        viewModel.BackRequested += () => Close();

        DataContext = viewModel;
    }
}