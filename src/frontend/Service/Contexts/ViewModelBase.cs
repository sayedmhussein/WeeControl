using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeeControl.Frontend.AppService.Interfaces;

[assembly: InternalsVisibleTo("ApplicationService.UnitTest")]
namespace WeeControl.Frontend.AppService.Contexts;

public abstract class ViewModelBase : IViewModelBase //UserApplication.Test.Integration
{
    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<string>? DisplayAlertEventHandler;
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void DisplayAlert(string message)
    {
        DisplayAlertEventHandler?.Invoke(this, message);
    }
}