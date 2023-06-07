using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class ContentsSubNavigationVM : NavigationBase
	{
		public ICommand SetContentsShowCommand { get; set; }
		public ICommand ContentsSettingCommand { get; set; }
		public ICommand CharAddCommand { get; set; }
		private void SetContentsShow(object obj) => CurrentView = new SetContentsVM();
		private void ContentsSetting(object obj) => CurrentView = new ContentsSettingVM();
		private void CharAdd(object obj) => CurrentView = new CharAddVM();
		public ContentsSubNavigationVM()
		{
			SetContentsShowCommand = new RelayCommand(SetContentsShow);
			ContentsSettingCommand = new RelayCommand(ContentsSetting);
			CharAddCommand = new RelayCommand(CharAdd);

			CurrentView = new SetContentsVM();
		}
	}
}
