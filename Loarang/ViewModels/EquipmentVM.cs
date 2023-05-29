using HtmlAgilityPack;
using Loarang.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Loarang.ViewModels
{
	class EquipmentVM : ViewModelBase
	{
		private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyIsImtpZCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyJ9.eyJpc3MiOiJodHRwczovL2x1ZHkuZ2FtZS5vbnN0b3ZlLmNvbSIsImF1ZCI6Imh0dHBzOi8vbHVkeS5nYW1lLm9uc3RvdmUuY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IjEwMDAwMDAwMDAwMDM1MjYifQ.L7jZBb7XM82NfzgeX9eoliGS5p1h1YQRYUCCMKDPApu8VKwrQmiBTBTL4Fo3F65X0nKxVcoLEy4aQH6FdsS9fgboTzs9iWygm7CMl3uuwfJAtlEZapbrSXVXbXRmor_Rw9lC1RWLvjh5PRkYAwkW196KeWKKU2hvf3DjsHlYH78ru8LTybL8J-6aTAS8qgpfh84H-MjCBPs5DJClEzK2sBPpxWe9mqPcOR1Gfe-BkwW179dubh8hk6Ys1jB2NNLVPKvV1qhGWoSXLTicl0hnm54pBZXN-I0P0kT_9x-cKr2wv6gzD1wmfngiPEk-elpb3jGCcabQNIVyw6N630faaw";

		private const int START_EQUIPMENT_CNT = 0;
		private const int END_EQUIPMENT_CNT = 6;
		private const int START_ACCESSORY_CNT = 6;
		private const int END_ACCESSORY_CNT = 11;
		private const int START_ETC_EQUIPMENT_CNT = 11;
		private const int END_ETC_EQUIPMENT_CNT = 13;

		BIEngrave bIEngrave;
		Equipment equipment;
		EtcEquipment etcEquipment;

		ObservableCollection<Equipment> equipments;
		ObservableCollection<Equipment> accessories;
		ObservableCollection<EtcEquipment> etcEquipments;
		ObservableCollection<BIEngrave> mountingEngraves;
		public EquipmentVM()
		{
			SetEvent();
		}
		private void SetEvent()
		{
			BattleInfoNavigation.SearchAlert -= SetEquipment;
			BattleInfoNavigation.SearchAlert += SetEquipment;
		}

		private async void SetEquipment(object sender, BattleInfoNavigation.CharacterNameArgs e)
		{
			equipments = new ObservableCollection<Equipment>();
			accessories = new ObservableCollection<Equipment>();
			etcEquipments = new ObservableCollection<EtcEquipment>();
			mountingEngraves = new ObservableCollection<BIEngrave>();

			try
			{
				using (var httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", API_KEY);

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

						ObservableCollection<BIEngrave> tempBIEngraves
							= new ObservableCollection<BIEngrave>();

						ObservableCollection<BIEngrave> tempMountingEngraves = new ObservableCollection<BIEngrave>();

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

							tempMountingEngraves.Add(bIEngrave);
						}

						MountingEngraves = tempMountingEngraves;
					}

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{e.Name}/equipment"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						ObservableCollection<Equipment> tempEquipments = new ObservableCollection<Equipment>();
						ObservableCollection<Equipment> tempAccessories = new ObservableCollection<Equipment>();
						ObservableCollection<EtcEquipment> tempEtcEquipments = new ObservableCollection<EtcEquipment>();

						for (int i = START_EQUIPMENT_CNT; i < END_EQUIPMENT_CNT; i++)
						{
							equipment = new Equipment();
							EquipmentName = jToken[i]["Name"].ToString();
							EquipmentType = jToken[i]["Type"].ToString();
							EquipmentImage = jToken[i]["Icon"].ToString();
							EquipmentTooltip = jToken[i]["Tooltip"].ToString();

							JToken tooltipJt = JToken.Parse(jToken[i]["Tooltip"].ToString());


							EquipmentQualityValue = Int32.Parse(tooltipJt["Element_001"]["value"]["qualityValue"].ToString());

							tempEquipments.Add(equipment);
						}

						for (int i = START_ACCESSORY_CNT; i < END_ACCESSORY_CNT; i++)
						{
							equipment = new Equipment();
							EquipmentName = jToken[i]["Name"].ToString();
							EquipmentType = jToken[i]["Type"].ToString();
							EquipmentImage = jToken[i]["Icon"].ToString();
							EquipmentTooltip = jToken[i]["Tooltip"].ToString();

							JToken tooltipJt = JToken.Parse(jToken[i]["Tooltip"].ToString());


							EquipmentQualityValue = Int32.Parse(tooltipJt["Element_001"]["value"]["qualityValue"].ToString());

							tempAccessories.Add(equipment);
						}

						for (int i = START_ETC_EQUIPMENT_CNT; i < END_ETC_EQUIPMENT_CNT; i++)
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

								for (int j = 0; j < imageNodes.Count; j++)
								{
									string imageNodeText = imageNodes[j].NextSibling.InnerText;

									if (imageNodeText.Contains("["))
									{
										string nextNodeText = imageNodes[j].NextSibling.NextSibling.InnerText;
										tempOption += nextNodeText;
									}

									else
									{
										imageNodeText = Regex.Replace(imageNodeText, @"[^가-힣]", "");
										tempOption += imageNodeText;
									}

									if (j != imageNodes.Count - 1)
									{
										tempOption += "-";
									}
								}

								EtcEquipmentOption = tempOption;

								tempEtcEquipments.Add(etcEquipment);
							}

							else if (EtcEquipmentName.Contains("돌"))
							{
								string tempOption = string.Empty;
								string firstOption = tooltipJt["Element_006"]["value"]["Element_000"]["contentStr"]
									["Element_000"]["contentStr"].ToString().Substring(23);

								firstOption = Regex.Replace(firstOption, @"[^0-9가-힣 ]", "");
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

								tempEtcEquipments.Add(etcEquipment);
							}
						}

						Equipments = tempEquipments;
						Accessories = tempAccessories;
						EtcEquipments = tempEtcEquipments;
					}
				}
			}
			catch (Exception ex)
			{
				// todo exception
			}
		}

		public ObservableCollection<Equipment> Equipments
		{
			get => equipments;
			set { equipments = value; OnPropertyChanged(nameof(Equipments)); }
		}

		public ObservableCollection<Equipment> Accessories
		{
			get => accessories;
			set { accessories = value; OnPropertyChanged(nameof(Accessories)); }
		}

		public ObservableCollection<EtcEquipment> EtcEquipments
		{
			get => etcEquipments;
			set { etcEquipments = value; OnPropertyChanged(nameof(EtcEquipments)); }
		}


		#region Engraves
		public ObservableCollection<BIEngrave> BIEngraves
		{
			get => mountingEngraves;
			set { mountingEngraves = value; OnPropertyChanged(nameof(BIEngraves)); }
		}
		public ObservableCollection<BIEngrave> MountingEngraves
		{
			get => mountingEngraves;
			set { mountingEngraves = value; OnPropertyChanged(nameof(MountingEngraves)); }
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
