using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class ContentsSubNavigationVM : NavigationBase
	{
		public ICommand ContentsSettingCommand { get; set; }
		public ICommand CharAddCommand { get; set; }
		private void ContentsSetting(object obj) => CurrentView = new ContentsSettingVM();
		private void CharAdd(object obj) => CurrentView = new CharAddVM();
		public ContentsSubNavigationVM()
		{
			ContentsSettingCommand = new RelayCommand(ContentsSetting);
			CharAddCommand = new RelayCommand(CharAdd);

			CurrentView = new ContentsSettingVM();
		}
	}
}
