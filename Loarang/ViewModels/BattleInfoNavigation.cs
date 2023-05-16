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
		private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyIsImtpZCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyJ9.eyJpc3MiOiJodHRwczovL2x1ZHkuZ2FtZS5vbnN0b3ZlLmNvbSIsImF1ZCI6Imh0dHBzOi8vbHVkeS5nYW1lLm9uc3RvdmUuY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IjEwMDAwMDAwMDAwMDM1MjYifQ.L7jZBb7XM82NfzgeX9eoliGS5p1h1YQRYUCCMKDPApu8VKwrQmiBTBTL4Fo3F65X0nKxVcoLEy4aQH6FdsS9fgboTzs9iWygm7CMl3uuwfJAtlEZapbrSXVXbXRmor_Rw9lC1RWLvjh5PRkYAwkW196KeWKKU2hvf3DjsHlYH78ru8LTybL8J-6aTAS8qgpfh84H-MjCBPs5DJClEzK2sBPpxWe9mqPcOR1Gfe-BkwW179dubh8hk6Ys1jB2NNLVPKvV1qhGWoSXLTicl0hnm54pBZXN-I0P0kT_9x-cKr2wv6gzD1wmfngiPEk-elpb3jGCcabQNIVyw6N630faaw";
		//private string PROFILES_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/{obj}/profiles";
		//private string EQUIPMENT_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/equipment";
		//private string COMBAT_SKILLS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/combat-skills";
		//private string ENGRAVINGS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/engravings";
		//private string CARDS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/cards";
		//private string GEMS_API_URL = $"https://developer-lostark.game.onstove.com/armories/characters/%EC%82%AC%EB%9E%91%EC%9D%98%EA%B2%B0%EC%A0%95%EC%B2%B4/gems";
		private const int START_EQUIPMENT_CNT = 0;
		private const int END_EQUIPMENT_CNT = 6;
		private const int START_ACCESORY_CNT = 6;
		private const int END_ACCESORY_CNT = 11;
		private const int START_ETC_EQUIPMENT_CNT = 11;
		private const int END_ETC_EQUIPMENT_CNT = 13;


		public ICommand SearchBattleInfoCommand { get; }	
		public ICommand ShowStatsCommand { get; set; }
		public ICommand ShowSkillTreeCommand { get; set; }

		BIProfiles bIProfiles;
		BIJewel bIJewel;
		BICharacteristic bICharacteristic;
		BIEngrave bIEngrave;
		BICardImage bICardImage;
		BICardDescription bICardDescription;
		Equipment equipment;
		EtcEquipment etcEquipment;

		private string searchCharName;

		private object _currentView;

		private void ShowStats(object obj) => CurrentView = new StatsVM();
		private void ShowSkillTree(object obj) => CurrentView = new SkillTreeVM();

		public static event EventHandler<EventArgs> SearchAlert;

		public BattleInfoNavigation()
		{
			bIProfiles = new BIProfiles();
			bICharacteristic = new BICharacteristic();
			bICardImage = new BICardImage();
			bICardDescription = new BICardDescription();

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

						ObservableCollection<BIJewel> tempBIJewels = new ObservableCollection<BIJewel>();
						BattleInfoShareStore.BIJewels = tempBIJewels;

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

							BattleInfoShareStore.BIJewels.Add(bIJewel);
							BattleInfoShareStore.BIJewels
								= new ObservableCollection<BIJewel>(BattleInfoShareStore.BIJewels.OrderByDescending(x => x.Priority));
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
						JToken engravingsToken = jToken["Engravings"];

						ObservableCollection<BIEngrave> tempBIEngraves
							= new ObservableCollection<BIEngrave>();

						BattleInfoShareStore.BIEngraves = tempBIEngraves;
						BattleInfoShareStore.MountingEngraves = new ObservableCollection<BIEngrave>();

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

						foreach (JToken jt in engravingsToken)
						{
							bIEngrave = new BIEngrave();
							EngraveName = jt["Name"].ToString();
							EngraveImage = jt["Icon"].ToString();
							
							JToken tooltipJt = JToken.Parse(jt["Tooltip"].ToString());

							HtmlDocument htmlDoc = new HtmlDocument();
							htmlDoc.LoadHtml(tooltipJt["Element_001"]["value"]["leftText"].ToString());
							HtmlNode imageNode = htmlDoc.DocumentNode.SelectSingleNode("font");

							EngraveLevel = imageNode.InnerText.Replace("각인 활성 포인트", "");

							EngraveName += "\n" + EngraveLevel;

							BattleInfoShareStore.MountingEngraves.Add(bIEngrave);
						}
					}

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{obj}/cards"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						JToken cardsToken = jToken["Cards"];
						JToken effectsToken = jToken["Effects"];

						BattleInfoShareStore.BICardImages = new ObservableCollection<BICardImage>();
						BattleInfoShareStore.BICardDescriptions = new ObservableCollection<BICardDescription>();

						foreach (JToken jt in cardsToken)
						{
							bICardImage = new BICardImage();

							CardImage = jt["Icon"].ToString();

							BattleInfoShareStore.BICardImages.Add(bICardImage);
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
								BattleInfoShareStore.BICardDescriptions.Add(bICardDescription);
							}							
						}
					}

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{obj}/equipment"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						BattleInfoShareStore.Equipments = new ObservableCollection<Equipment>();
						BattleInfoShareStore.Accessories = new ObservableCollection<Equipment>();
						BattleInfoShareStore.EtcEquipments = new ObservableCollection<EtcEquipment>();

						for (int i = START_EQUIPMENT_CNT; i < END_EQUIPMENT_CNT; i ++)
						{
							equipment = new Equipment();
							EquipmentName = jToken[i]["Name"].ToString();
							EquipmentType = jToken[i]["Type"].ToString();
							EquipmentImage = jToken[i]["Icon"].ToString();							
							EquipmentTooltip = jToken[i]["Tooltip"].ToString();

							JToken tooltipJt = JToken.Parse(jToken[i]["Tooltip"].ToString());


							EquipmentQualityValue = Int32.Parse(tooltipJt["Element_001"]["value"]["qualityValue"].ToString());

							BattleInfoShareStore.Equipments.Add(equipment);
						}

						for (int i = START_ACCESORY_CNT; i < END_ACCESORY_CNT; i ++)
						{
							equipment = new Equipment();
							EquipmentName = jToken[i]["Name"].ToString();
							EquipmentType = jToken[i]["Type"].ToString();
							EquipmentImage = jToken[i]["Icon"].ToString();
							EquipmentTooltip = jToken[i]["Tooltip"].ToString();

							JToken tooltipJt = JToken.Parse(jToken[i]["Tooltip"].ToString());


							EquipmentQualityValue = Int32.Parse(tooltipJt["Element_001"]["value"]["qualityValue"].ToString());

							BattleInfoShareStore.Accessories.Add(equipment);
						}

						for (int i = START_ETC_EQUIPMENT_CNT; i < END_ETC_EQUIPMENT_CNT; i ++)
						{
							etcEquipment = new EtcEquipment();
							EtcEquipmentName = jToken[i]["Name"].ToString();
							EtcEquipmentType = jToken[i]["Type"].ToString();
							EtcEquipmentImage = jToken[i]["Icon"].ToString();
							EtcEquipmentTooltip = jToken[i]["Tooltip"].ToString();

							JToken tooltipJt = JToken.Parse(jToken[i]["Tooltip"].ToString());

							if (EtcEquipmentName.Contains("팔찌"))
							{
								HtmlDocument htmlDoc = new HtmlDocument();
								htmlDoc.LoadHtml(tooltipJt["Element_004"]["value"]["Element_001"].ToString());
								HtmlNodeCollection imageNodes = htmlDoc.DocumentNode.SelectNodes("img");

								string tempOption = string.Empty;

								for (int j = 0; j < imageNodes.Count; j ++)
								{			
									string imageNodeText = imageNodes[j].NextSibling.InnerText;
																		
									if(imageNodeText.Contains("["))
									{
										string nextNodeText = imageNodes[j].NextSibling.NextSibling.InnerText;
										tempOption += nextNodeText;
									}

									else
									{
										imageNodeText = Regex.Replace(imageNodeText, @"[^가-힣]", "");
										tempOption += imageNodeText;
									}

									if(j != imageNodes.Count - 1)
									{
										tempOption += "-";
									}
								}

								EtcEquipmentOption = tempOption;

								BattleInfoShareStore.EtcEquipments.Add(etcEquipment);
							}

							else if (EtcEquipmentName.Contains("돌"))
							{
								string tempOption = string.Empty;
								string firstOption = tooltipJt["Element_006"]["value"]["Element_000"]["contentStr"]
									["Element_000"]["contentStr"].ToString().Substring(23);

								firstOption = Regex.Replace(firstOption, @"[^0-9가-힣 ]","");
								firstOption = firstOption.Replace("활성도 ", "");
								tempOption += firstOption + "\n";

								string secondOption = tooltipJt["Element_006"]["value"]["Element_000"]["contentStr"]
									["Element_001"]["contentStr"].ToString().Substring(23);

								secondOption = Regex.Replace(secondOption, @"[^0-9가-힣 ]", "");
								secondOption = secondOption.Replace("활성도 ", "");
								tempOption += secondOption + "\n";

								string thirdOption = tooltipJt["Element_006"]["value"]["Element_000"]["contentStr"]
									["Element_002"]["contentStr"].ToString().Substring(23);

								thirdOption = Regex.Replace(thirdOption, @"[^0-9가-힣 ]", "");
								thirdOption = thirdOption.Replace("활성도 ", "");
								tempOption += thirdOption;

								EtcEquipmentOption = tempOption;

								BattleInfoShareStore.EtcEquipments.Add(etcEquipment);
							}
						}
					}

					SearchAlert?.Invoke(this, new EventArgs());
				}
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

		#region Cards
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

		#region Equipment
		public string EquipmentName
		{
			get => equipment.EquipmentName;
			set { equipment.EquipmentName = value; OnPropertyChanged(nameof(EquipmentName)); }
		}
		public string EquipmentType
		{
			get => equipment.EquipmentType;
			set { equipment.EquipmentType = value; OnPropertyChanged(nameof(EquipmentType)); }
		}
		public string EquipmentImage
		{
			get => equipment.EquipmentImage;
			set { equipment.EquipmentImage = value; OnPropertyChanged(nameof(EquipmentImage)); }
		}
		public string EquipmentTooltip
		{
			get => equipment.EquipmentTooltip;
			set { equipment.EquipmentTooltip = value; OnPropertyChanged(nameof(EquipmentTooltip)); }
		}
		public int EquipmentQualityValue
		{
			get => equipment.EquipmentQualityValue;
			set { equipment.EquipmentQualityValue = value; OnPropertyChanged(nameof(EquipmentQualityValue)); }
		}
		public string EquipmentLevel
		{
			get => equipment.EquipmentLevel;
			set { equipment.EquipmentLevel = value; OnPropertyChanged(nameof(equipment.EquipmentLevel)); }
		}
		public string EtcEquipmentName
		{
			get => etcEquipment.EquipmentName;
			set { etcEquipment.EquipmentName = value; OnPropertyChanged(nameof(EtcEquipmentName)); }
		}
		public string EtcEquipmentType
		{
			get => etcEquipment.EquipmentType;
			set { etcEquipment.EquipmentType = value; OnPropertyChanged(nameof(EtcEquipmentType)); }
		}
		public string EtcEquipmentImage
		{
			get => etcEquipment.EquipmentImage;
			set { etcEquipment.EquipmentImage = value; OnPropertyChanged(nameof(EtcEquipmentImage)); }
		}
		public string EtcEquipmentTooltip
		{
			get => etcEquipment.EquipmentTooltip;
			set { etcEquipment.EquipmentTooltip = value; OnPropertyChanged(nameof(EtcEquipmentTooltip)); }
		}
		public string EtcEquipmentOption
		{
			get => etcEquipment.EtcEquipmentOption;
			set { etcEquipment.EtcEquipmentOption = value; OnPropertyChanged(nameof(EtcEquipmentOption)); }
		}
		#endregion
	}
}
