using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class BattleInfoSubNavigation : ViewModelBase
	{
		private object _currentView;	
		public ICommand ShowStatsCommand { get; set; }
		public ICommand ShowSkillTreeCommand { get; set; }
		private void ShowStats(object obj) => CurrentView = new StatsVM();
		private void ShowSkillTree(object obj) => CurrentView = new SkillTreeVM();
		public BattleInfoSubNavigation()
		{
			ShowStatsCommand = new RelayCommand(ShowStats);
			ShowSkillTreeCommand = new RelayCommand(ShowSkillTree);

			CurrentView = new StatsVM();
		}
		public object CurrentView
		{
			get { return _currentView; }
			set
			{ 
				_currentView = value;
				OnPropertyChanged(); 
			}
		}
	}
}
