using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

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