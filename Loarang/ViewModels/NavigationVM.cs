using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class NavigationVM : NavigationBase
	{
		public ICommand ContentsCommand { get; set; }		
		private void Content(object obj) => CurrentView = new ContentsVM();		
		public NavigationVM()
		{
			ContentsCommand = new RelayCommand(Content);

			CurrentView = new ContentsVM();
		}
	}
}
