using FontAwesome.Sharp;
using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class NavigationVM : NavigationBase
	{
		private string _caption;
		private IconChar _icon;

		public ICommand ShowHomeCommand { get; set; }
		public ICommand ShowContentsCommand { get; set; }
		public ICommand ShowCharacterCommand { get; set; }
		private void Content(object obj) => CurrentView = new ContentsVM();		
		public NavigationVM()
		{
			ShowHomeCommand = new RelayCommand(ShowHomeViewCommand);
			ShowContentsCommand = new RelayCommand(ShowContentsViewCommand);
			ShowCharacterCommand = new RelayCommand(ShowCharacterViewCommand);

			CurrentView = new HomeVM();
		}

		public string Caption
		{
			get { return _caption; }
			set { _caption = value; OnPropertyChanged(); }
		}

		public IconChar Icon
		{
			get { return _icon; }
			set { _icon = value; OnPropertyChanged(); }
		}

		private void ShowHomeViewCommand(object obj)
		{
			Caption = "홈";
			Icon = IconChar.Home;
			CurrentView = new HomeVM();
		}

		private void ShowContentsViewCommand(object obj)
		{
			Caption = "컨텐츠";
			Icon = IconChar.CalendarCheck;
			CurrentView = new ContentsVM();
		}
		private void ShowCharacterViewCommand(object obj)
		{
			Caption = "캐릭터";
			Icon = IconChar.User;
			CurrentView = new CharacterVM();
		}
	}
}
