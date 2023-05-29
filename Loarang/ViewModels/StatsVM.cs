using HtmlAgilityPack;
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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class StatsVM : ViewModelBase
	{
		private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyIsImtpZCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyJ9.eyJpc3MiOiJodHRwczovL2x1ZHkuZ2FtZS5vbnN0b3ZlLmNvbSIsImF1ZCI6Imh0dHBzOi8vbHVkeS5nYW1lLm9uc3RvdmUuY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IjEwMDAwMDAwMDAwMDM1MjYifQ.L7jZBb7XM82NfzgeX9eoliGS5p1h1YQRYUCCMKDPApu8VKwrQmiBTBTL4Fo3F65X0nKxVcoLEy4aQH6FdsS9fgboTzs9iWygm7CMl3uuwfJAtlEZapbrSXVXbXRmor_Rw9lC1RWLvjh5PRkYAwkW196KeWKKU2hvf3DjsHlYH78ru8LTybL8J-6aTAS8qgpfh84H-MjCBPs5DJClEzK2sBPpxWe9mqPcOR1Gfe-BkwW179dubh8hk6Ys1jB2NNLVPKvV1qhGWoSXLTicl0hnm54pBZXN-I0P0kT_9x-cKr2wv6gzD1wmfngiPEk-elpb3jGCcabQNIVyw6N630faaw";

		BIProfiles bIProfiles;
		BIJewel bIJewel;
		BICharacteristic bICharacteristic;
		BIEngrave bIEngrave;
		BICardImage bICardImage;
		BICardDescription bICardDescription;

		private object _currentView;

		EquipmentVM equipmentVM;

		public ICommand ShowEquipmentCommand { get; set; }

		ObservableCollection<BIJewel> bIJewels;
		ObservableCollection<BIEngrave> bIEngraves;
		ObservableCollection<BICardImage> bICardImages;
		ObservableCollection<BICardDescription> bICardDescriptions;

		public StatsVM()
		{
			equipmentVM = new EquipmentVM();
			EquipmentVMState.EquipmentVM = equipmentVM;

			bIProfiles = new BIProfiles();
			bICharacteristic = new BICharacteristic();
			
			CurrentView = EquipmentVMState.EquipmentVM;

			SetEvent();
		}

		private void SetEvent()
		{
			BattleInfoNavigation.SearchAlert -= SetBattleInfo;
			BattleInfoNavigation.SearchAlert += SetBattleInfo;
		}

		private async void SetBattleInfo(object sender, BattleInfoNavigation.CharacterNameArgs e)
		{
			bIJewels = new ObservableCollection<BIJewel>();
			bIEngraves = new ObservableCollection<BIEngrave>();
			bICardImages = new ObservableCollection<BICardImage>();
			bICardDescriptions = new ObservableCollection<BICardDescription>();

			try
			{
				using (var httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", API_KEY);

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{e.Name}/gems"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						jToken = jToken["Gems"];

						ObservableCollection<BIJewel> test = new ObservableCollection<BIJewel>();

						foreach (JToken jt in jToken)
						{
							bIJewel = new BIJewel();

							JewelName = jt["Name"].ToString();
							JewelImage = jt["Icon"].ToString();
							JewelLevel = Int32.Parse(jt["Level"].ToString());

							if (JewelName.Contains("멸"))
								Priority = 0.5 + JewelLevel;
							else
								Priority = JewelLevel;

							BIJewels.Add(bIJewel);
						}

						BIJewels = new ObservableCollection<BIJewel>(BIJewels.OrderByDescending(x => x.Priority));
					}


					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{e.Name}/profiles"))
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
					}

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{e.Name}/engravings"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);
						JToken effectsJToken = jToken["Effects"];
						JToken engravingsToken = jToken["Engravings"];

						ObservableCollection<BIEngrave> tempBIEngraves = new ObservableCollection<BIEngrave>();

						foreach (JToken jt in effectsJToken)
						{
							bIEngrave = new BIEngrave();
							EngraveName = jt["Name"].ToString().Substring(0, jt["Name"].ToString().Length - 6);
							EngraveLevel = jt["Name"].ToString().Substring(jt["Name"].ToString().Length - 6);

							string skillImageName = EngraveName.Replace(" ", string.Empty);
							skillImageName = skillImageName.Replace(":", string.Empty);

							EngraveImage = $"..\\Resources\\{skillImageName}.png";

							tempBIEngraves.Add(bIEngrave);
						}

						BIEngraves = tempBIEngraves;
					}

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{e.Name}/cards"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);
						JToken cardsToken = jToken["Cards"];
						JToken effectsToken = jToken["Effects"];

						ObservableCollection<BICardImage> tempBICardImages = new ObservableCollection<BICardImage>();
						ObservableCollection<BICardDescription> tempBICardDescriptions = new ObservableCollection<BICardDescription>();

						foreach (JToken jt in cardsToken)
						{
							bICardImage = new BICardImage();

							CardImage = jt["Icon"].ToString();

							tempBICardImages.Add(bICardImage);
						}

						foreach (JToken jt in effectsToken)
						{
							foreach (JToken itemsJt in jt["Items"])
							{
								bICardDescription = new BICardDescription();

								CardSetName = itemsJt["Name"].ToString();

								for (int i = 0; i < itemsJt["Description"].ToString().Length; i++)
								{
									if (i % 27 == 0)
									{
										CardSetOption += "\n";
									}

									CardSetOption += itemsJt["Description"].ToString()[i];
								}
								tempBICardDescriptions.Add(bICardDescription);
							}
						}

						BICardImages = tempBICardImages;
						BICardDescriptions = tempBICardDescriptions;
					}
				}
			}

			catch (Exception ex)
			{

			}

			EquipmentVMState.EquipmentVM = equipmentVM;
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
		public ObservableCollection<BIJewel> BIJewels
		{
			get => bIJewels;
			set { bIJewels = value; OnPropertyChanged(nameof(BIJewels)); }
		}

		public string JewelName
		{
			get => bIJewel.JewelName;
			set { bIJewel.JewelName = value; OnPropertyChanged(nameof(JewelName)); }
		}
		public string JewelImage
		{
			get => bIJewel.JewelImage;
			set { bIJewel.JewelImage = value; OnPropertyChanged(nameof(JewelImage)); }
		}
		public int JewelLevel
		{
			get => bIJewel.JewelLevel;
			set { bIJewel.JewelLevel = value; OnPropertyChanged(nameof(JewelLevel)); }
		}

		public double Priority
		{
			get => bIJewel.Priority;
			set { bIJewel.Priority = value; OnPropertyChanged(nameof(Priority)); }
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

		#region Cards
		public ObservableCollection<BICardImage> BICardImages
		{
			get => bICardImages;
			set { bICardImages = value; OnPropertyChanged(nameof(BICardImages)); }
		}
		public ObservableCollection<BICardDescription> BICardDescriptions
		{
			get => bICardDescriptions;
			set { bICardDescriptions = value; OnPropertyChanged(nameof(BICardDescriptions)); }
		}

		public string CardImage
		{
			get => bICardImage.CardImage;
			set { bICardImage.CardImage = value; OnPropertyChanged(nameof(CardImage)); }
		}
		public string CardSetName
		{
			get => bICardDescription.CardSetName;
			set { bICardDescription.CardSetName = value; OnPropertyChanged(nameof(CardSetName)); }
		}
		public string CardSetOption
		{
			get => bICardDescription.CardSetOption;
			set { bICardDescription.CardSetOption = value; OnPropertyChanged(nameof(CardSetOption)); }
		}
		#endregion

		#region Current View
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}
		#endregion
	}
}
