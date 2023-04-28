using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class BattleInfoSubNavigation : NavigationBase
	{
		public ICommand ShowStatsCommand { get; set; }
		public ICommand ShowSkillCommand { get; set; }
		private void ShowStats(object obj) => CurrentView = new StatsVM();
		private void ShowSkill(object obj) => CurrentView = new SkillVM();
		public BattleInfoSubNavigation()
		{
			ShowStatsCommand = new RelayCommand(ShowStats);
			ShowSkillCommand = new RelayCommand(ShowSkill);

			CurrentView = new StatsVM();
		}
	}
}
