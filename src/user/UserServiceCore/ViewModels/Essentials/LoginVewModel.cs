using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeeControl.User.UserServiceCore.Annotations;

namespace WeeControl.User.UserServiceCore.ViewModels.Essentials;

public class LoginVewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}