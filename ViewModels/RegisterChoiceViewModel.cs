using System;
using CommunityToolkit.Mvvm.Input;

namespace TreningAplikacija.ViewModels;

public partial class RegisterChoiceViewModel : ViewModelBase
{
    public event Action? ClientChosen;
    public event Action? TrainerChosen;
    public event Action? BackRequested;

    [RelayCommand]
    private void ChooseClient() => ClientChosen.Invoke();
    
    [RelayCommand]
    private void ChooseTrainer() => TrainerChosen.Invoke();
    
    [RelayCommand]
    private void Back() => BackRequested.Invoke();
}