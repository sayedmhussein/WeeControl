using System.ComponentModel;

namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IViewModelBase : INotifyPropertyChanged
{
    public bool IsLoading { get; set; }
}