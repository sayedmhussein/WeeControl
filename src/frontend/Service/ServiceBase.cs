using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeeControl.Frontend.Service.Interfaces;

[assembly: InternalsVisibleTo("ApplicationService.UnitTest")]
namespace WeeControl.Frontend.Service;

public abstract class ServiceBase : IViewModelBase //UserApplication.Test.Integration
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
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}