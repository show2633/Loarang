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
		public ICommand ShowBattleInfoCommand { get; set; }
		public NavigationVM()
		{
			ShowHomeCommand = new RelayCommand(ShowHomeViewCommand);
			ShowContentsCommand = new RelayCommand(ShowContentsViewCommand);
			ShowBattleInfoCommand = new RelayCommand(ShowBattleInfoViewCommand);

			Init();
		}

		private void Init()
		{
			Caption = "홈";
			Icon = IconChar.Home;
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
		private void ShowBattleInfoViewCommand(object obj)
		{
			Caption = "전투정보실";
			Icon = IconChar.User;
			CurrentView = new BattleInfoVM();
		}
	}
}
