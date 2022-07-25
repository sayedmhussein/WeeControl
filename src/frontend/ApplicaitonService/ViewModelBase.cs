using System.ComponentModel;

namespace WeeControl.Frontend.ApplicationService;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        protected set
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