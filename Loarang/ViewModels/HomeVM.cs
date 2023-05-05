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
	public class HomeVM : ViewModelBase
	{
		private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyIsImtpZCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyJ9.eyJpc3MiOiJodHRwczovL2x1ZHkuZ2FtZS5vbnN0b3ZlLmNvbSIsImF1ZCI6Imh0dHBzOi8vbHVkeS5nYW1lLm9uc3RvdmUuY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IjEwMDAwMDAwMDAwMDM1MjYifQ.L7jZBb7XM82NfzgeX9eoliGS5p1h1YQRYUCCMKDPApu8VKwrQmiBTBTL4Fo3F65X0nKxVcoLEy4aQH6FdsS9fgboTzs9iWygm7CMl3uuwfJAtlEZapbrSXVXbXRmor_Rw9lC1RWLvjh5PRkYAwkW196KeWKKU2hvf3DjsHlYH78ru8LTybL8J-6aTAS8qgpfh84H-MjCBPs5DJClEzK2sBPpxWe9mqPcOR1Gfe-BkwW179dubh8hk6Ys1jB2NNLVPKvV1qhGWoSXLTicl0hnm54pBZXN-I0P0kT_9x-cKr2wv6gzD1wmfngiPEk-elpb3jGCcabQNIVyw6N630faaw";
		private const string NOTICE_API_URL = "https://developer-lostark.game.onstove.com/news/notices?type=%EA%B3%B5%EC%A7%80";
		private const string EVENT_API_URL = "https://developer-lostark.game.onstove.com/news/events";
		private const string CALENDAR_API_URL = "https://developer-lostark.game.onstove.com/gamecontents/calendar";
		private const string CHALLENGE_ABYSS_API_URL = "https://developer-lostark.game.onstove.com/gamecontents/challenge-abyss-dungeons";
		private const string CHALLENGE_GUARDIAN_API_URL = "https://developer-lostark.game.onstove.com/gamecontents/challenge-guardian-raids";

		private Home _home;

		public ICommand ShowHomeContentsCommand { get; set; }
		public HomeVM()
		{
			_home = new Home();
			ShowHomeContentsCommand = new RelayCommand(ShowHomeContents);
		}

		private async void ShowHomeContents(object obj)
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", API_KEY);

				using (var request = new HttpRequestMessage(new HttpMethod("GET"), NOTICE_API_URL))
				{
					request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);
					request.Content = new StringContent("");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);
					string res = await response.Content.ReadAsStringAsync();

					JToken jToken = JToken.Parse(res);

					for (int i = 0; i < 8; i++)
					{
						HomeNotice homeNotice = new HomeNotice();
						homeNotice.NoticeText = jToken[i]["Title"].ToString();
						homeNotice.Url = jToken[i]["Link"].ToString();
						_home.HomeNoticeList.Add(homeNotice);
					}
				}

				using (var request = new HttpRequestMessage(new HttpMethod("GET"), EVENT_API_URL))
				{
					request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);
					request.Content = new StringContent("");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);
					string res = await response.Content.ReadAsStringAsync();

					JToken jToken = JToken.Parse(res);

					for (int i = 0; i < 6; i++)
					{
						HomeEvent homeEvent = new HomeEvent();
						homeEvent.EventText = jToken[i]["Title"].ToString();
						homeEvent.Url = jToken[i]["Link"].ToString();
						homeEvent.EventImage = jToken[i]["Thumbnail"].ToString();
						_home.HomeEventList.Add(homeEvent);
					}
				}

				using (var request = new HttpRequestMessage(new HttpMethod("GET"), CALENDAR_API_URL))
				{
					request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);
					request.Content = new StringContent("");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);
					string res = await response.Content.ReadAsStringAsync();

					JToken jToken = JToken.Parse(res);

					foreach (JToken jt in jToken)
					{
						if (jt["CategoryName"].ToString() == "모험 섬")
						{
							if (jt["StartTimes"].ToString().Contains(DateTime.Now.ToString("yyyy-MM-dd")))
							{
								string[] tempTimes = jt["StartTimes"].ToString().Split(',');
								string firstTime = string.Empty;
								string islandTime = string.Empty;

								foreach (string time in tempTimes)
								{
									if (time.Contains(DateTime.Now.ToString("yyyy-MM-dd")))
									{
										firstTime = time;

										break;
									}
								}
								HomeAdventureIsland homeAdventureIsland = new HomeAdventureIsland();

								if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
									DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
								{
									if(DateTime.Now.Hour <= 13 && firstTime.Contains("09:00:00"))
									{
										homeAdventureIsland.ContentsName = jt["ContentsName"].ToString();
										homeAdventureIsland.ContentsImage = jt["ContentsIcon"].ToString();

										foreach (JToken subJt in jt["RewardItems"])
										{
											if (subJt["StartTimes"].ToString().Contains(firstTime))
											{
												if (subJt["Name"].ToString() == "실링" ||
													subJt["Name"].ToString() == "골드" ||
													subJt["Name"].ToString() == "대양의 주화 상자" ||
													subJt["Name"].ToString() == "전설 ~ 고급 카드 팩")
												{
													homeAdventureIsland.RewardItems = subJt["Icon"].ToString();
												}
											}
										}

										_home.HomeAdventureIslandList.Add(homeAdventureIsland);
									}

									else if(DateTime.Now.Hour > 13 && firstTime.Contains("19:00:00"))
									{
										homeAdventureIsland.ContentsName = jt["ContentsName"].ToString();
										homeAdventureIsland.ContentsImage = jt["ContentsIcon"].ToString();

										foreach (JToken subJt in jt["RewardItems"])
										{
											if (subJt["StartTimes"].ToString().Contains(firstTime))
											{
												if (subJt["Name"].ToString() == "실링" ||
													subJt["Name"].ToString() == "골드" ||
													subJt["Name"].ToString() == "대양의 주화 상자" ||
													subJt["Name"].ToString() == "전설 ~ 고급 카드 팩")
												{
													homeAdventureIsland.RewardItems = subJt["Icon"].ToString();
												}
											}
										}

										_home.HomeAdventureIslandList.Add(homeAdventureIsland);
									}
								}

								else
								{
									homeAdventureIsland.ContentsName = jt["ContentsName"].ToString();
									homeAdventureIsland.ContentsImage = jt["ContentsIcon"].ToString();

									foreach (JToken subJt in jt["RewardItems"])
									{
										if (subJt["StartTimes"].ToString().Contains(firstTime))
										{
											if (subJt["Name"].ToString() == "실링" ||
												subJt["Name"].ToString() == "골드" ||
												subJt["Name"].ToString() == "대양의 주화 상자" ||
												subJt["Name"].ToString() == "전설 ~ 고급 카드 팩")
											{
												homeAdventureIsland.RewardItems = subJt["Icon"].ToString();
											}
										}
									}

									_home.HomeAdventureIslandList.Add(homeAdventureIsland);
								}
							}
						}
					}
				}


				using (var request = new HttpRequestMessage(new HttpMethod("GET"), CHALLENGE_ABYSS_API_URL))
				{
					request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);
					request.Content = new StringContent("");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);
					string res = await response.Content.ReadAsStringAsync();

					JToken jToken = JToken.Parse(res);

					foreach (JToken jt in jToken)
					{
						HomeChallengeAbyss homeChallengeAbyss = new HomeChallengeAbyss();
						homeChallengeAbyss.ContentsName = jt["Name"].ToString();
						homeChallengeAbyss.ContentsImage = jt["Image"].ToString();

						_home.HomeChallengeAbyssList.Add(homeChallengeAbyss);
					}
				}

				using (var request = new HttpRequestMessage(new HttpMethod("GET"), CHALLENGE_GUARDIAN_API_URL))
				{
					request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);
					request.Content = new StringContent("");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);
					string res = await response.Content.ReadAsStringAsync();

					JToken jToken = JToken.Parse(res);

					foreach (JToken jt in jToken["Raids"])
					{
						HomeChallengeGuardian homeChallengeGuardian = new HomeChallengeGuardian();
						homeChallengeGuardian.GuardianName = jt["Name"].ToString();
						homeChallengeGuardian.GuardianImage = jt["Image"].ToString();

						_home.homeChallengeGuardianList.Add(homeChallengeGuardian);
					}
				}
			}
		}

		public ObservableCollection<HomeNotice> HomeNotices
		{
			get => _home.HomeNoticeList;
			set
			{
				_home.HomeNoticeList = value;
				OnPropertyChanged(nameof(HomeNotices));
			}
		}
		public ObservableCollection<HomeEvent> HomeEvents
		{
			get => _home.HomeEventList;
			set
			{
				_home.HomeEventList = value;
				OnPropertyChanged(nameof(HomeEvents));
			}
		}

		public ObservableCollection<HomeAdventureIsland> HomeAdventureIslands
		{
			get => _home.HomeAdventureIslandList;
			set
			{
				_home.HomeAdventureIslandList = value;
				OnPropertyChanged(nameof(HomeAdventureIslands));
			}
		}

		public ObservableCollection<HomeChallengeAbyss> HomeChallengeAbysses
		{
			get => _home.HomeChallengeAbyssList;
			set
			{
				_home.HomeChallengeAbyssList = value;
				OnPropertyChanged(nameof(HomeChallengeAbysses));
			}
		}

		public ObservableCollection<HomeChallengeGuardian> HomeChallengeGuardians
		{
			get => _home.homeChallengeGuardianList;
			set
			{
				_home.homeChallengeGuardianList = value;
				OnPropertyChanged(nameof(HomeChallengeGuardians));
			}
		}

	}
}
