using System.Windows.Input;

namespace WeeControl.User.UserServiceCore.Services;

internal class RelayCommand : ICommand
{
    readonly Action<object> _execute;
    readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _execute = execute ?? throw new NullReferenceException("Relay Command Class Received Null");
        _canExecute = canExecute;
    }

    public RelayCommand(Action<object> execute) : this(execute, null)
    {
    }

    //Todo: Need to check CommandManager which is not supported in .net standard.
    public event EventHandler CanExecuteChanged;
    //{
    //    add { CommandManager.RequerySuggested += value; }
    //    remove { CommandManager.RequerySuggested -= value; }
    //}

    public bool CanExecute(object parameter)
    {
        return _canExecute == null ? true : _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute.Invoke(parameter);
    }
}