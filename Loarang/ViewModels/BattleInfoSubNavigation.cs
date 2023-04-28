using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class BattleInfoSubNavigation : NavigationBase
	{
		public ICommand ShowStatsCommand { get; set; }
		private void ShowStats(object obj) => CurrentView = new StatsVM();
		public BattleInfoSubNavigation()
		{
			ShowStatsCommand = new RelayCommand(ShowStats);

			CurrentView = new StatsVM();
		}
	}
}
