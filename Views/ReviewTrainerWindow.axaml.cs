using System;
using Avalonia.Controls;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class ReviewTrainerWindow : Window
{
    public ReviewTrainerWindow() : this(Guid.Empty, new Trainer())
    {
    }
    public ReviewTrainerWindow(Guid clientId, Trainer trainer)
    {
        InitializeComponent();

        ReviewTrainerViewModel viewModel = new ReviewTrainerViewModel(clientId, trainer);
        viewModel.BackRequested += () => Close();
        viewModel.ReviewSubmitted += () => Close();

        DataContext = viewModel;
    }
}