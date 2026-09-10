using Avalonia.Controls;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        public AdminWindow(Admin admin) : this()
        {
            DataContext = new AdminViewModel(admin);
        }
    }
}