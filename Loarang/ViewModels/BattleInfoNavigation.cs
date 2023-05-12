using Loarang.Command;
using Loarang.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	public class BattleInfoNavigation : ViewModelBase
	{
		private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyIsImtpZCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyJ9.eyJpc3MiOiJodHRwczovL2x1ZHkuZ2FtZS5vbnN0b3ZlLmNvbSIsImF1ZCI6Imh0dHBzOi8vbHVkeS5nYW1lLm9uc3RvdmUuY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IjEwMDAwMDAwMDAwMDM1MjYifQ.L7jZBb7XM82NfzgeX9eoliGS5p1h1YQRYUCCMKDPApu8VKwrQmiBTBTL4Fo3F65X0nKxVcoLEy4aQH6FdsS9fgboTzs9iWygm7CMl3uuwfJAtlEZapbrSXVXbXRmor_Rw9lC1RWLvjh5PRkYAwkW196KeWKKU2hvf3DjsHlYH78ru8LTybL8J-6aTAS8qgpfh84H-MjCBPs5DJClEzK2sBPpxWe9mqPcOR1Gfe-BkwW179dubh8hk6Ys1jB2NNLVPKvV1qhGWoSXLTicl0hnm54pBZXN-I0P0kT_9x-cKr2wv6gzD1wmfngiPEk-elpb3jGCcabQNIVyw6N630faaw";
		//private string PROFILES_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/{obj}/profiles";
		//private string EQUIPMENT_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/equipment";
		//private string COMBAT_SKILLS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/combat-skills";
		//private string ENGRAVINGS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/engravings";
		//private string CARDS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/cards";
		//private string GEMS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/gems";
		private const int MAX_JEWEL_CNT = 11;

		public ICommand SearchBattleInfoCommand { get; }
		public ICommand ShowStatsCommand { get; set; }
		public ICommand ShowSkillTreeCommand { get; set; }

		BIProfiles bIProfiles;
		BIJewels bIJewels;
		BICharacteristic bICharacteristic;
		BIEngrave bIEngrave;

		private string searchCharName;

		private object _currentView;
	
		private void ShowStats(object obj) => CurrentView = new StatsVM();
		private void ShowSkillTree(object obj) => CurrentView = new SkillTreeVM();

		public static event EventHandler<EventArgs> SearchAlert;

		public BattleInfoNavigation()
		{
			bIProfiles = new BIProfiles();
			bICharacteristic = new BICharacteristic();

			searchCharName = string.Empty;

			SearchBattleInfoCommand = new RelayCommand((param) => SearchBattleInfo(param));
			ShowStatsCommand = new RelayCommand(ShowStats);
			ShowSkillTreeCommand = new RelayCommand(ShowSkillTree);

			CurrentView = new StatsVM();
		}
		private async void SearchBattleInfo(object obj)
		{
			try
			{
				using (var httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", API_KEY);

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{obj}/profiles"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						Server = jToken["ServerName"].ToString();
						CharacterName = jToken["CharacterName"].ToString();
						ClassName = jToken["CharacterClassName"].ToString();
						CharacterLevel = Int32.Parse(jToken["CharacterLevel"].ToString());
						ItemMaxLevel = jToken["ItemMaxLevel"].ToString();
						ExpeditionLevel = Int32.Parse(jToken["ExpeditionLevel"].ToString());
						Title = jToken["Title"].ToString();
						GuildName = jToken["GuildName"].ToString();
						PvpGradeName = jToken["PvpGradeName"].ToString();
						TownName = jToken["TownName"].ToString();
						CharacterImage = jToken["CharacterImage"].ToString();

						jToken = jToken["Stats"];

						
						Crit = Int32.Parse(jToken[0]["Value"].ToString());
						Specialization = Int32.Parse(jToken[1]["Value"].ToString());
						Domination = int.Parse(jToken[2]["Value"].ToString());
						Swiftness = Int32.Parse(jToken[3]["Value"].ToString());
						Endurance = Int32.Parse(jToken[4]["Value"].ToString());
						Expertise = Int32.Parse(jToken[5]["Value"].ToString());
						MaxHP = Int32.Parse(jToken[6]["Value"].ToString());
						AtkPower = Int32.Parse(jToken[7]["Value"].ToString());

						BattleInfoShareStore.BIProfiles = bIProfiles;
						BattleInfoShareStore.BICharacteristic = bICharacteristic;
					}

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{obj}/gems"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						jToken = jToken["Gems"];

						ObservableCollection<BIJewels> tempBIJewels = new ObservableCollection<BIJewels>();
						BattleInfoShareStore.BIJewels = tempBIJewels;

						foreach(JToken jt in jToken)
						{ 
							bIJewels = new BIJewels();

							JewelName = jt["Name"].ToString();
							JewelImage = jt["Icon"].ToString();
							JewelLevel = Int32.Parse(jt["Level"].ToString());

							if (JewelName.Contains("멸"))
								Priority = 0.5 + JewelLevel;
							else
								Priority = JewelLevel;

							BattleInfoShareStore.BIJewels.Add(bIJewels);
							BattleInfoShareStore.BIJewels
								= new ObservableCollection<BIJewels>(BattleInfoShareStore.BIJewels.OrderByDescending(x => x.Priority));
						}
					}

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{obj}/engravings"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						JToken effectsJToken = jToken["Effects"];

						ObservableCollection<BIEngrave> tempBIEngraves 
							= new ObservableCollection<BIEngrave>();
						BattleInfoShareStore.BIEngraves = tempBIEngraves;

						foreach (JToken jt in effectsJToken)
						{
							bIEngrave = new BIEngrave();
							EngraveName = jt["Name"].ToString().Substring(0, jt["Name"].ToString().Length - 6);
							EngraveLevel = jt["Name"].ToString().Substring(jt["Name"].ToString().Length - 6);

							string skillImageName = EngraveName.Replace(" ", string.Empty);
							skillImageName = skillImageName.Replace(":", string.Empty);

							EngraveImage = $"..\\Resources\\{skillImageName}.png";

							BattleInfoShareStore.BIEngraves.Add(bIEngrave);
						}
					}
					SearchAlert?.Invoke(this, new EventArgs());
				}
			}			
			catch(Exception e)
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

		#region Jewels
		public string JewelName
		{
			get => bIJewels.JewelName;
			set { bIJewels.JewelName = value; OnPropertyChanged(nameof(JewelName)); }
		}
		public string JewelImage
		{
			get => bIJewels.JewelImage;
			set { bIJewels.JewelImage = value; OnPropertyChanged(nameof(JewelImage)); }
		}
		public int JewelLevel
		{
			get => bIJewels.JewelLevel;
			set { bIJewels.JewelLevel = value; OnPropertyChanged(nameof(JewelLevel)); }
		}

		public double Priority
		{
			get => bIJewels.Priority;
			set { bIJewels.Priority = value; OnPropertyChanged(nameof(Priority)); }
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
		public string EngraveName
		{
			get => bIEngrave.EngraveName;
			set { bIEngrave.EngraveName = value; OnPropertyChanged(nameof(EngraveName)); }
		}
		public string EngraveImage
		{
			get => bIEngrave.EngraveImage;
			set { bIEngrave.EngraveImage = value; OnPropertyChanged(nameof(EngraveImage)); }
		}
		public string EngraveLevel
		{
			get => bIEngrave.EngraveLevel;
			set { bIEngrave.EngraveLevel = value; OnPropertyChanged(nameof(EngraveLevel)); }
		}
		#endregion
	}
}
