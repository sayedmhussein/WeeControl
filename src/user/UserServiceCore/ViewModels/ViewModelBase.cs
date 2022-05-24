using System.ComponentModel;

namespace WeeControl.User.UserServiceCore.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public bool IsLoading { get; protected set; } = false;
    
    public event PropertyChangedEventHandler? PropertyChanged;
}