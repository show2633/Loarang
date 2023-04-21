using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Loarang.Command
{
	class RelayCommand : ICommand
	{
		private readonly Action<object> _excuteAction;
		public event EventHandler CanExecuteChanged;
		public RelayCommand(Action<object> excutionAction)
		{
			_excuteAction = excutionAction;
		}

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) => _excuteAction(parameter);

		public event EventHandler CanExcuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}
