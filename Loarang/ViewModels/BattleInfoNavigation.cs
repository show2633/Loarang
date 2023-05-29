using Loarang.Command;
using Loarang.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Web;
using System.IO;
using HtmlAgilityPack;

namespace Loarang.ViewModels
{
	public class BattleInfoNavigation : ViewModelBase
	{
		public ICommand SearchBattleInfoCommand { get; }	
		public ICommand ShowStatsCommand { get; set; }
		public ICommand ShowSkillTreeCommand { get; set; }

		private string searchCharName;

		private object _currentView;

		StatsVM statsVM;
		SkillTreeVM skillTreeVM;

		private void ShowStats(object obj) => CurrentView = StatsVMState.StatsVM;
		private void ShowSkillTree(object obj) => CurrentView = SkillTreeVMState.SkillTreeVM;

		public static event EventHandler<CharacterNameArgs> SearchAlert;

		public BattleInfoNavigation()
		{
			statsVM = new StatsVM();
			skillTreeVM = new SkillTreeVM();

			StatsVMState.StatsVM = statsVM;
			SkillTreeVMState.SkillTreeVM = skillTreeVM;

			searchCharName = string.Empty;

			SearchBattleInfoCommand = new RelayCommand((param) => SearchBattleInfo(param));
			ShowStatsCommand = new RelayCommand(ShowStats);
			ShowSkillTreeCommand = new RelayCommand(ShowSkillTree);

			CurrentView = StatsVMState.StatsVM;
		}
		private void SearchBattleInfo(object obj)
		{
			try
			{
				SearchAlert?.Invoke(this, new CharacterNameArgs()
				{
					Name = obj.ToString()
				});

				StatsVMState.StatsVM = statsVM;
				SkillTreeVMState.SkillTreeVM = skillTreeVM;
			}
			catch (Exception e)
			{
				// todo exception
			}
		}

		#region Profiles
		public string SearchCharName
		{
			get => searchCharName;
			set { searchCharName = value; OnPropertyChanged(nameof(SearchCharName)); }
		}
		#endregion

		#region Current View
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}
		#endregion
		public class CharacterNameArgs
		{
			public string Name { get; set; }
		}
	}
}


