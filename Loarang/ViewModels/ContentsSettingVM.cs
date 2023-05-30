using Loarang.Command;
using Loarang.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	public class ContentsSettingVM : ViewModelBase
	{
		private const string contentsFileName = "contents.csv";

		ObservableCollection<Content> dailyContents;
		ObservableCollection<Content> commanderRaidContents;
		ObservableCollection<Content> abyssContents;
		ObservableCollection<Content> abyssRaidContents;
		ObservableCollection<Content> guildIslandContents;
		ObservableCollection<Content> weeklyContents;
		ObservableCollection<Content> weeklyEtcContents;

		ContentsSetting _contentSetting;

		public ICommand ContentsSaveCommand { get; }

		public ContentsSettingVM()
		{
			_contentSetting = new ContentsSetting();

			ContentsSaveCommand = new RelayCommand(ContentsSave);

			ContentsSetting();
		}

		private void ContentsSave(object obj)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(contentsFileName))
				{
					foreach(var dailyContent in dailyContents)
					{
						sw.WriteLine("{0},{1},{2}", "dailyContent", dailyContent.ContentName, dailyContent.ContentFlag);
					}

					foreach (var commanderRaidContent in commanderRaidContents)
					{
						sw.WriteLine("{0},{1},{2}", "commanderRaidContent", commanderRaidContent.ContentName, commanderRaidContent.ContentFlag);
					}

					foreach (var abyssContent in abyssContents)
					{
						sw.WriteLine("{0},{1},{2}", "abyssContent", abyssContent.ContentName, abyssContent.ContentFlag);
					}

					foreach (var abyssRaidContent in abyssRaidContents)
					{
						sw.WriteLine("{0},{1},{2}", "abyssRaidContent", abyssRaidContent.ContentName, abyssRaidContent.ContentFlag);
					}

					foreach (var guildIslendContent in guildIslandContents)
					{
						sw.WriteLine("{0},{1},{2}", "guildIslendContent", guildIslendContent.ContentName, guildIslendContent.ContentFlag);
					}

					foreach (var weeklyContent in weeklyContents)
					{
						sw.WriteLine("{0},{1},{2}", "weeklyContent", weeklyContent.ContentName, weeklyContent.ContentFlag);
					}

					foreach (var weeklyEtcContent in weeklyEtcContents)
					{
						sw.WriteLine("{0},{1},{2}", "weeklyEtcContent", weeklyEtcContent.ContentName, weeklyEtcContent.ContentFlag);
					}
				}
			}

			catch(Exception e)
			{

			}
		}

		private void ContentsSetting()
		{
			dailyContents = new ObservableCollection<Content>();
			dailyContents.Add(new Content("일일 에포나", true));
			dailyContents.Add(new Content("카오스 던전", true));
			dailyContents.Add(new Content("가디언 토벌", true));
			dailyContents.Add(new Content("모험 섬", true));
			dailyContents.Add(new Content("필드 보스", true));
			dailyContents.Add(new Content("카오스 게이트", true));
			dailyContents.Add(new Content("이벤트 섬", true));
			DailyContents = dailyContents;

			commanderRaidContents = new ObservableCollection<Content>();
			commanderRaidContents.Add(new Content("발탄", true));
			commanderRaidContents.Add(new Content("비아키스", true));
			commanderRaidContents.Add(new Content("쿠크세이튼", true));
			commanderRaidContents.Add(new Content("아브렐슈드", true));
			commanderRaidContents.Add(new Content("일리아칸", true));
			CommanderRaidContents = commanderRaidContents;

			abyssContents = new ObservableCollection<Content>();
			abyssContents.Add(new Content("기본 6종", true));
			abyssContents.Add(new Content("낙원 3종", true));
			abyssContents.Add(new Content("오레하", true));
			abyssContents.Add(new Content("카양겔", true));
			abyssContents.Add(new Content("상아탑", true));
			AbyssContents = abyssContents;


			abyssRaidContents = new ObservableCollection<Content>();
			abyssRaidContents.Add(new Content("아르고스", true));
			AbyssRaidContents = abyssRaidContents;

			guildIslandContents = new ObservableCollection<Content>();
			guildIslandContents.Add(new Content("슬라임", true));
			guildIslandContents.Add(new Content("메데이아", true));
			GuildIslandContents = guildIslandContents;

			weeklyContents = new ObservableCollection<Content>();
			weeklyContents.Add(new Content("유령선", true));
			WeeklyContents = weeklyContents;

			weeklyEtcContents = new ObservableCollection<Content>();
			weeklyEtcContents.Add(new Content("엘가시아 보석 교환", true));
			weeklyEtcContents.Add(new Content("페이토 카드 교환", true));
			weeklyEtcContents.Add(new Content("페르마타 카드 교환", true));
			weeklyEtcContents.Add(new Content("혈석 상자 교환", true));
			weeklyEtcContents.Add(new Content("길드 의뢰", true));
			weeklyEtcContents.Add(new Content("주간 에포나", true));
			weeklyEtcContents.Add(new Content("길드 토벌", true));
			weeklyEtcContents.Add(new Content("이벤트 상점 교환", true));
			WeeklyEtcContents = weeklyEtcContents;

			//collection.Add(new Content("[기타]"));
			//collection.Add(new Content("큐브")); // 추후 수정
		}

		public ObservableCollection<Content> DailyContents
		{
			get => _contentSetting.DailyContents;
			set
			{
				_contentSetting.DailyContents = value;
				OnPropertyChanged(nameof(DailyContents));
			}
		}

		public ObservableCollection<Content> CommanderRaidContents
		{
			get => _contentSetting.CommanderRaidContents;
			set
			{
				_contentSetting.CommanderRaidContents = value;
				OnPropertyChanged(nameof(CommanderRaidContents));
			}
		}

		public ObservableCollection<Content> AbyssContents
		{
			get => _contentSetting.AbyssContents;
			set
			{
				_contentSetting.AbyssContents = value;
				OnPropertyChanged(nameof(AbyssContents));
			}
		}

		public ObservableCollection<Content> AbyssRaidContents
		{
			get => _contentSetting.AbyssRaidContents;
			set
			{
				_contentSetting.AbyssRaidContents = value;
				OnPropertyChanged(nameof(AbyssRaidContents));
			}
		}

		public ObservableCollection<Content> GuildIslandContents
		{
			get => _contentSetting.GuildIslandContents;
			set
			{
				_contentSetting.GuildIslandContents = value;
				OnPropertyChanged(nameof(GuildIslandContents));
			}
		}

		public ObservableCollection<Content> WeeklyContents
		{
			get => _contentSetting.WeeklyContents;
			set
			{
				_contentSetting.WeeklyContents = value;
				OnPropertyChanged(nameof(WeeklyContents));
			}
		}

		public ObservableCollection<Content> WeeklyEtcContents
		{
			get => _contentSetting.WeeklyEtcContents;
			set
			{
				_contentSetting.WeeklyEtcContents = value;
				OnPropertyChanged(nameof(WeeklyEtcContents));
			}
		}
	}
}
