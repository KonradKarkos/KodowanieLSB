using System;
using System.Windows.Input;

namespace LSBEncoding.Commands
{
    /// <summary>
    /// Base command that can be bound to e.g button
    /// </summary>
    public class BindableCommand : ICommand
    {
        /// <summary>
        /// Action that should be executed on command run
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// Handler that should determine if action can be executed
        /// </summary>
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="execute">Action that should be executed on command run</param>
        /// <param name="canExecute">Handler that should determine if action can be executed</param>
        public BindableCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Parameter CanExecute action. This, being a parameterless command, just returns parameterless version
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// Executes methos provided in constructor
        /// </summary>
        /// <returns>Indicator if action can be executed, if is null then always returns true</returns>
        public bool CanExecute()
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }

            return true;
        }

        /// <summary>
        /// Parameter Execute action. This, being a parameterless command, just returns parameterless version
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            Execute();
        }

        /// <summary>
        /// Executes method provided in constructor
        /// </summary>
        public void Execute()
        {
            _execute();
        }

        /// <summary>
        /// Event fired when CanExecute result has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises CanExecuteChanged event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
