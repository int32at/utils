using System;
using System.Windows.Input;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Windows.Wpf.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute.ThrowIfNull("execute");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.IsNull() || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged(object parameter = null)
        {
            var handler = CanExecuteChanged;
            if (handler.IsNotNull())
                handler(this, EventArgs.Empty);
        }
    }
}
