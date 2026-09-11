using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class RateTrainingWindow : Window
{
    public RateTrainingWindow() : this(new TrainingSession())
    {
    }
    public RateTrainingWindow(TrainingSession session)
    {
        InitializeComponent();

        RateTrainingViewModel viewModel = new RateTrainingViewModel(session);
        viewModel.BackRequested += () => Close();
        viewModel.Completed += () => Close();

        DataContext = viewModel;
    }
}