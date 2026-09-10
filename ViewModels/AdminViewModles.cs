using CommunityToolkit.Mvvm.ComponentModel;
using TreningAplikacija.Models;

namespace TreningAplikacija.ViewModels;

public partial class AdminViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcomeMessage;

    public AdminViewModel(Admin admin)
    {
        _welcomeMessage = $"Dobrodošli, {admin.Name}";
    }
}