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

		private Home _home;

		public ICommand ShowHomeContentsCommand { get; set; }
		public HomeVM()
		{
			_home = new Home();
			ShowHomeContentsCommand = new RelayCommand(ShowHomeContents);
		}

		private async void ShowHomeContents(object obj)
		{
			using(var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", API_KEY);

				using(var request = new HttpRequestMessage(new HttpMethod("GET"), NOTICE_API_URL))
				{
					request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);
					request.Content = new StringContent("");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);
					string res = await response.Content.ReadAsStringAsync();

					JToken jToken = JToken.Parse(res);

					for(int i = 0; i < 8; i ++)
					{
						HomeNotice homeNotice = new HomeNotice();
						homeNotice.NoticeText = jToken[i]["Title"].ToString();
						homeNotice.Url = jToken[i]["Link"].ToString();
						_home.HomeNoticeList.Add(homeNotice);
					}
				}

				using(var request = new HttpRequestMessage(new HttpMethod("GET"), EVENT_API_URL))
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
	}
}
