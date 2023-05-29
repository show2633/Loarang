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

namespace Loarang.ViewModels
{
	public class SkillTreeVM : ViewModelBase
	{
		private const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyIsImtpZCI6IktYMk40TkRDSTJ5NTA5NWpjTWk5TllqY2lyZyJ9.eyJpc3MiOiJodHRwczovL2x1ZHkuZ2FtZS5vbnN0b3ZlLmNvbSIsImF1ZCI6Imh0dHBzOi8vbHVkeS5nYW1lLm9uc3RvdmUuY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IjEwMDAwMDAwMDAwMDM1MjYifQ.L7jZBb7XM82NfzgeX9eoliGS5p1h1YQRYUCCMKDPApu8VKwrQmiBTBTL4Fo3F65X0nKxVcoLEy4aQH6FdsS9fgboTzs9iWygm7CMl3uuwfJAtlEZapbrSXVXbXRmor_Rw9lC1RWLvjh5PRkYAwkW196KeWKKU2hvf3DjsHlYH78ru8LTybL8J-6aTAS8qgpfh84H-MjCBPs5DJClEzK2sBPpxWe9mqPcOR1Gfe-BkwW179dubh8hk6Ys1jB2NNLVPKvV1qhGWoSXLTicl0hnm54pBZXN-I0P0kT_9x-cKr2wv6gzD1wmfngiPEk-elpb3jGCcabQNIVyw6N630faaw";

		ObservableCollection<Skill> skills;
		Skill skill;

		public SkillTreeVM()
		{
			skills = new ObservableCollection<Skill>();

			BattleInfoNavigation.SearchAlert += SetSkills;
		}
		public async void SetSkills(object sender, BattleInfoNavigation.CharacterNameArgs e)
		{
			skills = new ObservableCollection<Skill>();

			try
			{
				using (var httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", API_KEY);

					using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://developer-lostark.game.onstove.com/armories/characters/{e.Name}/combat-skills"))
					{
						request.Headers.TryAddWithoutValidation("x-apikey", API_KEY);

						request.Content = new StringContent("");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
						string res = await response.Content.ReadAsStringAsync();

						JToken jToken = JToken.Parse(res);

						ObservableCollection<Skill> tempSkills = new ObservableCollection<Skill>();

						foreach (JToken jt in jToken)
						{
							if (Int32.Parse(jt["Level"].ToString()) != 1)
							{
								skill = new Skill();
								skill.SkillName = jt["Name"].ToString();
								skill.SkillImage = jt["Icon"].ToString();
								skill.SkillLevel = Int32.Parse(jt["Level"].ToString());

								if (jt["Rune"].ToString() != string.Empty)
								{
									skill.RuneName = jt["Rune"]["Name"].ToString();
									skill.RuneImage = jt["Rune"]["Icon"].ToString();
								}

								foreach (JToken tripodJt in jt["Tripods"])
								{
									if ((Int32.Parse(tripodJt["Tier"].ToString()) == 0)
										&& Boolean.Parse(tripodJt["IsSelected"].ToString()))
									{
										skill.FirstTripodName = tripodJt["Name"].ToString();
										skill.FirstTripodImage = tripodJt["Icon"].ToString();
									}
									else if ((Int32.Parse(tripodJt["Tier"].ToString()) == 1)
										&& Boolean.Parse(tripodJt["IsSelected"].ToString()))
									{
										skill.SecondTripodName = tripodJt["Name"].ToString();
										skill.SecondTripodImage = tripodJt["Icon"].ToString();
									}
									else if ((Int32.Parse(tripodJt["Tier"].ToString()) == 2)
										&& Boolean.Parse(tripodJt["IsSelected"].ToString()))
									{
										skill.ThirdTripodName = tripodJt["Name"].ToString();
										skill.ThirdTripodImage = tripodJt["Icon"].ToString();
									}
								}
								tempSkills.Add(skill);

								Skills = tempSkills;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{

			}
		}

		public ObservableCollection<Skill> Skills
		{
			get => skills;
			set { skills = value; OnPropertyChanged(nameof(Skills)); }
		}

		public string SkillImage
		{
			get => skill.SkillImage;
			set { skill.SkillImage = value; OnPropertyChanged(nameof(SkillImage)); }
		}
		public string SkillName
		{
			get => skill.SkillName;
			set { skill.SkillName = value; OnPropertyChanged(nameof(SkillName)); }
		}
		public int SkillLevel
		{
			get => skill.SkillLevel;
			set { skill.SkillLevel = value; OnPropertyChanged(nameof(SkillLevel)); }
		}
		public string SelectedTripods
		{
			get => SelectedTripods;
			set { SelectedTripods = value; OnPropertyChanged(nameof(SelectedTripods)); }
		}
		public string FirstTripodImage
		{
			get => skill.FirstTripodImage;
			set { skill.FirstTripodImage = value; OnPropertyChanged(nameof(FirstTripodImage)); }
		}
		public string FirstTripodName
		{
			get => skill.FirstTripodName;
			set { skill.FirstTripodName = value; OnPropertyChanged(nameof(FirstTripodName)); }
		}
		public string SecondTripodImage
		{
			get => skill.SecondTripodImage;
			set { skill.SecondTripodImage = value; OnPropertyChanged(nameof(SecondTripodImage)); }
		}
		public string SecondTripodName
		{
			get => skill.SecondTripodName;
			set { skill.SecondTripodName = value; OnPropertyChanged(nameof(SecondTripodName)); }
		}
		public string ThirdTripodImage
		{
			get => skill.ThirdTripodImage;
			set { skill.ThirdTripodImage = value; OnPropertyChanged(nameof(ThirdTripodImage)); }
		}
		public string ThirdTripodName
		{
			get => skill.ThirdTripodName;
			set { skill.ThirdTripodName = value; OnPropertyChanged(nameof(ThirdTripodName)); }
		}
		public string RuneImage
		{
			get => skill.RuneImage;
			set { skill.RuneImage = value; OnPropertyChanged(nameof(RuneImage)); }
		}
		public string RuneName
		{
			get => skill.RuneName;
			set { skill.RuneName = value; OnPropertyChanged(nameof(RuneName)); }
		}
	}
}
