using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace WPFUtils
{
    public class Command : ICommand
    {
        
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canexecute;

        public Command(Action<object> execute) : this(execute, null) { }
        public Command(Action<object> execute, Func<object, bool> canexecute)
        {
            _execute = execute;
            _canexecute = canexecute;
        }

        public bool CanExecute(object parameter)
        {
            return (_canexecute == null || _canexecute(parameter));
        }

        public event EventHandler CanExecuteChanged = (s, e) => { };

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, new EventArgs());
        }
    }

    public class Command<T> : ICommand
    {

        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canexecute;

        public Command(Action<T> execute) : this(execute, null) { }
        public Command(Action<T> execute, Func<T, bool> canexecute)
        {
            _execute = execute;
            _canexecute = canexecute;
        }

        public bool CanExecute(object parameter)
        {
            return (_canexecute == null || _canexecute((T)parameter));
            
        }

        public event EventHandler CanExecuteChanged = (s, e) => { };

        public void Execute(object parameter)
        {            
            _execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, new EventArgs());
        }
    }


}
