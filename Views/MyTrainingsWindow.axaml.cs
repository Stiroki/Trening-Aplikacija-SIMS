using Avalonia;
using TreningAplikacija.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class MyTrainingsWindow : Window
{
    public MyTrainingsWindow() : this(new Client())
    {
    }
    
    public MyTrainingsWindow(Client client)
    {
        InitializeComponent();

        MyTrainingsViewModel viewModel = new MyTrainingsViewModel(client);
        viewModel.BackRequested += () => Close();
        viewModel.RateRequested += OnRateRequested;

        DataContext = viewModel;
    }

    private void OnRateRequested(TrainingSession session)
    {
        RateTrainingWindow rateWindow = new RateTrainingWindow(session);
        rateWindow.Closed += (_, _) =>
        {
            var viewModel = (MyTrainingsViewModel)DataContext!;
            viewModel.RefreshSessions();
        };
        rateWindow.Show();
    }
}