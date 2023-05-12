using Loarang.Command;
using Loarang.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class StatsSubNavigation : ViewModelBase
	{
		private object _currentView;
		public ICommand ShowEquipmentCommand { get; set; }

		BIProfiles bIProfiles;
		ObservableCollection<BIJewels> bIJewels;
		BICharacteristic bICharacteristic;
		ObservableCollection<BIEngrave> bIEngraves;
		private void ShowEquipment(object obj) => CurrentView = new EquipmentVM();

		public StatsSubNavigation()
		{
			bIProfiles = new BIProfiles();
			bIJewels = new ObservableCollection<BIJewels>();
			bICharacteristic = new BICharacteristic();
			bIEngraves = new ObservableCollection<BIEngrave>();

			ShowEquipmentCommand = new RelayCommand(ShowEquipment);

			CurrentView = new EquipmentVM();

			BattleInfoNavigation.SearchAlert += SetBattleInfo;
		}

		public void SetBattleInfo(object sender, EventArgs e)
		{
			// Profiles
			Server = BattleInfoShareStore.BIProfiles.Server;
			CharacterName = BattleInfoShareStore.BIProfiles.CharacterName;
			ClassName = BattleInfoShareStore.BIProfiles.ClassName;
			CharacterLevel = BattleInfoShareStore.BIProfiles.CharacterLevel;
			ItemMaxLevel = BattleInfoShareStore.BIProfiles.ItemMaxLevel;
			ExpeditionLevel = BattleInfoShareStore.BIProfiles.ExpeditionLevel;
			Title = BattleInfoShareStore.BIProfiles.Title;
			GuildName = BattleInfoShareStore.BIProfiles.GuildName;
			PvpGradeName = BattleInfoShareStore.BIProfiles.PvpGradeName;
			TownName = BattleInfoShareStore.BIProfiles.TownName;
			CharacterImage = BattleInfoShareStore.BIProfiles.CharacterImage;

			// Characteristic
			Crit = BattleInfoShareStore.BICharacteristic.Crit;
			Specialization = BattleInfoShareStore.BICharacteristic.Specialization;
			Domination = BattleInfoShareStore.BICharacteristic.Domination;
			Swiftness = BattleInfoShareStore.BICharacteristic.Swiftness;
			Endurance = BattleInfoShareStore.BICharacteristic.Endurance;
			Expertise = BattleInfoShareStore.BICharacteristic.Expertise;
			AtkPower = BattleInfoShareStore.BICharacteristic.AtkPower;
			MaxHP = BattleInfoShareStore.BICharacteristic.MaxHP;

			// Jewel
			BIJewels = BattleInfoShareStore.BIJewels;

			// Engrave
			BIEngraves = BattleInfoShareStore.BIEngraves;
		}
		
		#region Profiles
		public string Server
		{
			get => bIProfiles.Server;
			set { bIProfiles.Server = value; OnPropertyChanged(nameof(Server)); }
		}
		public string CharacterName
		{
			get => bIProfiles.CharacterName;
			set { bIProfiles.CharacterName = value; OnPropertyChanged(nameof(CharacterName)); }
		}
		public string ClassName
		{
			get => bIProfiles.ClassName;
			set { bIProfiles.ClassName = value; OnPropertyChanged(nameof(ClassName)); }
		}
		public int CharacterLevel
		{
			get => bIProfiles.CharacterLevel;
			set { bIProfiles.CharacterLevel = value; OnPropertyChanged(nameof(CharacterLevel)); }
		}
		public string ItemMaxLevel
		{
			get => bIProfiles.ItemMaxLevel;
			set { bIProfiles.ItemMaxLevel = value; OnPropertyChanged(nameof(ItemMaxLevel)); }
		}
		public int ExpeditionLevel
		{
			get => bIProfiles.ExpeditionLevel;
			set { bIProfiles.ExpeditionLevel = value; OnPropertyChanged(nameof(ExpeditionLevel)); }
		}
		public string Title
		{
			get => bIProfiles.Title;
			set { bIProfiles.Title = value; OnPropertyChanged(nameof(Title)); }
		}
		public string GuildName
		{
			get => bIProfiles.GuildName;
			set { bIProfiles.GuildName = value; OnPropertyChanged(nameof(GuildName)); }
		}
		public string PvpGradeName
		{
			get => bIProfiles.PvpGradeName;
			set { bIProfiles.PvpGradeName = value; OnPropertyChanged(nameof(PvpGradeName)); }
		}
		public string TownName
		{
			get => bIProfiles.TownName;
			set { bIProfiles.TownName = value; OnPropertyChanged(nameof(TownName)); }
		}
		public string CharacterImage
		{
			get => bIProfiles.CharacterImage;
			set { bIProfiles.CharacterImage = value; OnPropertyChanged(nameof(CharacterImage)); }
		}
		#endregion

		#region Jewel
		public ObservableCollection<BIJewels> BIJewels
		{
			get => bIJewels;
			set { bIJewels = value; OnPropertyChanged(nameof(BIJewels)); }
		}
		#endregion

		#region Current View
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}
		#endregion

		#region Characteristic
		public int Crit
		{
			get => bICharacteristic.Crit;
			set { bICharacteristic.Crit = value; OnPropertyChanged(nameof(Crit)); }
		}
		public int Specialization
		{
			get => bICharacteristic.Specialization;
			set { bICharacteristic.Specialization = value; OnPropertyChanged(nameof(Specialization)); }
		}
		public int Domination
		{
			get => bICharacteristic.Domination;
			set { bICharacteristic.Domination = value; OnPropertyChanged(nameof(Domination)); }
		}
		public int Swiftness
		{
			get => bICharacteristic.Swiftness;
			set { bICharacteristic.Swiftness = value; OnPropertyChanged(nameof(Swiftness)); }
		}
		public int Endurance
		{
			get => bICharacteristic.Endurance;
			set { bICharacteristic.Endurance = value; OnPropertyChanged(nameof(Endurance)); }
		}
		public int Expertise
		{
			get => bICharacteristic.Expertise;
			set { bICharacteristic.Expertise = value; OnPropertyChanged(nameof(Expertise)); }
		}
		public int MaxHP
		{
			get => bICharacteristic.MaxHP;
			set { bICharacteristic.MaxHP = value; OnPropertyChanged(nameof(MaxHP)); }
		}
		public int AtkPower
		{
			get => bICharacteristic.AtkPower;
			set { bICharacteristic.AtkPower = value; OnPropertyChanged(nameof(AtkPower)); }
		}
		#endregion

		#region Engraves
		public ObservableCollection<BIEngrave> BIEngraves
		{
			get => bIEngraves;
			set { bIEngraves = value; OnPropertyChanged(nameof(BIEngraves)); }
		}
		#endregion
	}
}
