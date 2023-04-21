using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.ViewModels
{
	public class NavigationBase : ViewModelBase
	{
		private object _currentView;
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value;  OnPropertyChanged(); }
		}
	}
}
