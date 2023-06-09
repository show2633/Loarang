﻿using HtmlAgilityPack;
using Loarang.Models;
using Loarang.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

						for (int i = 0; i < jToken.Children().Count(); i++)
						{
							if (jToken[i]["Type"].ToString() == "무기"
								|| jToken[i]["Type"].ToString() == "투구"
								|| jToken[i]["Type"].ToString() == "상의"
								|| jToken[i]["Type"].ToString() == "하의"
								|| jToken[i]["Type"].ToString() == "장갑"
								|| jToken[i]["Type"].ToString() == "어깨")
							{
								equipment = new Equipment();
								EquipmentName = jToken[i]["Name"].ToString();
								EquipmentImage = jToken[i]["Icon"].ToString();
								EquipmentTooltip = JsonUtility.ConvertToPlainText(jToken[i]["Tooltip"].ToString());

								JToken tooltipJt = JToken.Parse(EquipmentTooltip);

								EquipmentType = tooltipJt["Element_001"]["value"]["leftStr0"].ToString() + "\n";
								EquipmentType += tooltipJt["Element_001"]["value"]["leftStr2"].ToString();

								string elixirElement_1 = string.Empty;
								string elixirElement_2 = string.Empty;
								string eqpSetElement = string.Empty;

								if (EquipmentName.Contains("+25"))
								{
									elixirElement_1 = "Element_007";
									elixirElement_2 = "Element_008";
									eqpSetElement = "Element_009";
								}

								else
								{
									elixirElement_1 = "Element_008";
									elixirElement_2 = "Element_009";
									eqpSetElement = "Element_010";
								}

								if (tooltipJt[elixirElement_1]["type"].ToString() == "ItemPartBox"
										|| tooltipJt[elixirElement_1]["type"].ToString() == "SingleTextBox")
								{
									EquipmentTooltip = tooltipJt["Element_005"]["value"]["Element_001"].ToString() + "\n\n";
									EquipmentTooltip += tooltipJt["Element_006"]["value"]["Element_001"].ToString() + "\n\n";

									if (tooltipJt[elixirElement_1]["type"].ToString() == "ItemPartBox")
										EquipmentTooltip += tooltipJt[elixirElement_1]["value"]["Element_001"].ToString();
								}

								else
								{
									EquipmentTooltip = tooltipJt["Element_005"]["value"]["Element_001"].ToString() + "\n\n";
									EquipmentTooltip += tooltipJt["Element_006"]["value"]["Element_001"].ToString() + "\n\n";

									string[] tempElixirOptions = tooltipJt[elixirElement_1]["value"]["Element_000"]["contentStr"]
										["Element_000"]["contentStr"].ToString().Split('\n');

									for (int j = 0; j < tempElixirOptions.Length; j++)
									{
										for (int k = 0; k < tempElixirOptions[j].Length; k++)
										{
											if (k != 0 && k % 28 == 0)
											{
												tempElixirOptions[j] = tempElixirOptions[j].Insert(k, "\n");
											}
										}

										EquipmentTooltip += tempElixirOptions[j] + "\n";
									}

									EquipmentTooltip += "\n";

									try
									{
										tempElixirOptions = tooltipJt[elixirElement_1]["value"]["Element_000"]["contentStr"]
										["Element_001"]["contentStr"].ToString().Split('\n');
									}

									catch (Exception ex)
									{
										if (tooltipJt[elixirElement_2]["type"].ToString() == "ItemPartBox")
										{
											EquipmentTooltip += tooltipJt[elixirElement_2]["value"]["Element_001"].ToString();
										}

										else
										{
											EquipmentTooltip += tooltipJt[elixirElement_2]["value"]["Element_001"].ToString();
										}

										EquipmentQualityValue = Int32.Parse(tooltipJt["Element_001"]["value"]["qualityValue"].ToString());

										tempEquipments.Add(equipment);

										continue;
									}

									for (int j = 0; j < tempElixirOptions.Length; j++)
									{
										for (int k = 0; k < tempElixirOptions[j].Length; k++)
										{
											if (k != 0 && k % 28 == 0)
											{
												tempElixirOptions[j] = tempElixirOptions[j].Insert(k, "\n");
											}
										}

										EquipmentTooltip += tempElixirOptions[j] + "\n";
									}

									EquipmentTooltip += "\n";

									if (tooltipJt[elixirElement_2]["type"].ToString() == "ItemPartBox")
									{
										EquipmentTooltip += tooltipJt[elixirElement_2]["value"]["Element_001"].ToString();
									}

									else
									{
										string[] tempElixirSetOptions = tooltipJt[elixirElement_2]["value"]["Element_000"]["contentStr"]
											["Element_000"]["contentStr"].ToString().Split('\n');

										for (int j = 0; j < tempElixirSetOptions.Length; j++)
										{
											for (int k = 0; k < tempElixirSetOptions[j].Length; k++)
											{
												if (k != 0 && k % 28 == 0)
												{
													tempElixirSetOptions[j] = tempElixirSetOptions[j].Insert(k, "\n");
												}
											}

											EquipmentTooltip += tempElixirSetOptions[j] + "\n";
										}

										EquipmentTooltip += "\n";

										tempElixirSetOptions = tooltipJt[elixirElement_2]["value"]["Element_000"]["contentStr"]
											["Element_001"]["contentStr"].ToString().Split('\n');

										for (int j = 0; j < tempElixirSetOptions.Length; j++)
										{
											for (int k = 0; k < tempElixirSetOptions[j].Length; k++)
											{
												if (k != 0 && k % 28 == 0)
												{
													tempElixirSetOptions[j] = tempElixirSetOptions[j].Insert(k, "\n");
												}
											}

											EquipmentTooltip += tempElixirSetOptions[j] + "\n";
										}

										EquipmentTooltip += "\n";

										EquipmentTooltip += tooltipJt[eqpSetElement]["value"]["Element_001"].ToString();
									}
								}

								EquipmentQualityValue = Int32.Parse(tooltipJt["Element_001"]["value"]["qualityValue"].ToString());

								tempEquipments.Add(equipment);
							}
							
							else if (jToken[i]["Type"].ToString() == "반지"
								|| jToken[i]["Type"].ToString() == "귀걸이"
								|| jToken[i]["Type"].ToString() == "목걸이")
							{
								equipment = new Equipment();
								EquipmentName = jToken[i]["Name"].ToString();
								EquipmentImage = jToken[i]["Icon"].ToString();
								EquipmentTooltip = JsonUtility.ConvertToPlainText(jToken[i]["Tooltip"].ToString());

								JToken tooltipJt = JToken.Parse(EquipmentTooltip);

								EquipmentType = tooltipJt["Element_001"]["value"]["leftStr0"].ToString();

								EquipmentTooltip = tooltipJt["Element_004"]["value"]["Element_001"].ToString() + "\n"; // 힘 민 지
								EquipmentTooltip += tooltipJt["Element_005"]["value"]["Element_001"].ToString() + "\n\n";
								EquipmentTooltip += tooltipJt["Element_006"]["value"]["Element_000"]["contentStr"]
									["Element_000"]["contentStr"].ToString();
								EquipmentTooltip += tooltipJt["Element_006"]["value"]["Element_000"]["contentStr"]
									["Element_001"]["contentStr"].ToString() + "\n";
								EquipmentTooltip += tooltipJt["Element_006"]["value"]["Element_000"]["contentStr"]
									["Element_002"]["contentStr"].ToString();

								EquipmentQualityValue = Int32.Parse(tooltipJt["Element_001"]["value"]["qualityValue"].ToString());

								tempAccessories.Add(equipment);
							}

							else if (jToken[i]["Type"].ToString() == "팔찌"
								|| jToken[i]["Type"].ToString() == "어빌리티 스톤")
							{
								etcEquipment = new EtcEquipment();
								EtcEquipmentName = jToken[i]["Name"].ToString();
								EtcEquipmentImage = jToken[i]["Icon"].ToString();
								EtcEquipmentTooltip = JsonUtility.ConvertToPlainText(jToken[i]["Tooltip"].ToString());

								JToken tooltipJt = JToken.Parse(EtcEquipmentTooltip);

								EtcEquipmentType = tooltipJt["Element_001"]["value"]["leftStr0"].ToString();
								if (EtcEquipmentName.Contains("팔찌"))
								{
									tooltipJt = JToken.Parse(jToken[i]["Tooltip"].ToString());

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

									tooltipJt = JToken.Parse(EtcEquipmentTooltip);

									string[] tempBraceletOptions = tooltipJt["Element_004"]["value"]["Element_001"].ToString().Split('\n');

									EtcEquipmentTooltip = string.Empty;

									for (int j = 0; j < tempBraceletOptions.Length; j++)
									{
										for (int k = 0; k < tempBraceletOptions[j].Length; k++)
										{
											tempBraceletOptions[j] = tempBraceletOptions[j].Trim();

											if (k != 0 && k % 28 == 0)
											{												
												tempBraceletOptions[j] = tempBraceletOptions[j].Insert(k, "\n");
											}
										}

										EtcEquipmentTooltip += tempBraceletOptions[j] + "\n";
									}

									tempEtcEquipments.Add(etcEquipment);
								}

								else if (EtcEquipmentName.Contains("돌"))
								{
									string stoneHpElement = string.Empty;
									string stoneExtraHpElement = string.Empty;
									string stoneElement = string.Empty;

									if (tooltipJt["Element_005"]["type"].ToString() == "ItemPartBox" )
									{
										stoneElement = "Element_006";
										stoneExtraHpElement = "Element_005";
									}

									else
									{
										stoneElement = "Element_005";
									}

									stoneHpElement = "Element_004";

									EtcEquipmentTooltip = tooltipJt[stoneHpElement]["value"]["Element_001"].ToString() + "\n";

									if (stoneExtraHpElement != string.Empty)
										EtcEquipmentTooltip += tooltipJt[stoneExtraHpElement]["value"]["Element_001"].ToString() + "\n\n";
									else
										EtcEquipmentTooltip += "\n";

									string tempOption = string.Empty;
									string firstOption = tooltipJt[stoneElement]["value"]["Element_000"]["contentStr"]
										["Element_000"]["contentStr"].ToString();
									tempOption += firstOption;

									string secondOption = tooltipJt[stoneElement]["value"]["Element_000"]["contentStr"]
										["Element_001"]["contentStr"].ToString();
									tempOption += secondOption;

									string thirdOption = tooltipJt[stoneElement]["value"]["Element_000"]["contentStr"]
										["Element_002"]["contentStr"].ToString();
									tempOption += thirdOption;

									EtcEquipmentTooltip += tempOption;

									EtcEquipmentOption = tempOption;

									tempEtcEquipments.Add(etcEquipment);
								}
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
