using System;
using System.Windows.Input;

namespace Ongar.Mvvm
{
    public class DistributedCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;


        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;



        public DistributedCommandBase(Predicate<object> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public DistributedCommandBase(Action<object> execute)
        {
            _execute = execute;
        }



        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

}
