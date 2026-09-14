using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class SetPreferencesWindow : Window
{
    public SetPreferencesWindow() : this(new TrainerRequest(), "")
    {
    }
    public SetPreferencesWindow(TrainerRequest request, string TrainerName)
    {
        InitializeComponent();

        SetPreferencesViewModel viewModel = new SetPreferencesViewModel(request, TrainerName);
        viewModel.BackRequested += () => Close();
        viewModel.Saved += () => Close();

        DataContext = viewModel;
    }
}