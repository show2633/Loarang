using Loarang.Models;
using System.Collections.ObjectModel;

namespace Loarang.ViewModels
{
	public class ContentsSettingVM : ViewModelBase
	{
		ContentsSetting _contentSetting;

		public ContentsSettingVM()
		{
			_contentSetting = new ContentsSetting();

			ContentsSetting();
		}

		private void ContentsSetting()
		{
			ObservableCollection<Content>  dailyContents = new ObservableCollection<Content>();
			dailyContents.Add(new Content("일일 에포나", true));
			dailyContents.Add(new Content("카오스 던전", true));
			dailyContents.Add(new Content("가디언 토벌", true));
			dailyContents.Add(new Content("모험 섬", true));
			dailyContents.Add(new Content("필드 보스", true));
			dailyContents.Add(new Content("카오스 게이트", true));
			dailyContents.Add(new Content("이벤트 섬", true));
			DailyContents = dailyContents;

			ObservableCollection<Content> commanderRaidContents = new ObservableCollection<Content>();
			commanderRaidContents.Add(new Content("발탄", true));
			commanderRaidContents.Add(new Content("비아키스", true));
			commanderRaidContents.Add(new Content("쿠크세이튼", true));
			commanderRaidContents.Add(new Content("아브렐슈드", true));
			commanderRaidContents.Add(new Content("일리아칸", true));
			CommanderRaidContents = commanderRaidContents;

			ObservableCollection<Content> abyssContents = new ObservableCollection<Content>();
			abyssContents.Add(new Content("기본 6종", true));
			abyssContents.Add(new Content("낙원 3종", true));
			abyssContents.Add(new Content("오레하", true));
			abyssContents.Add(new Content("카양겔", true));
			abyssContents.Add(new Content("상아탑", true));
			AbyssContents = abyssContents;


			ObservableCollection<Content> abyssRaidContents = new ObservableCollection<Content>();
			abyssRaidContents.Add(new Content("아르고스", true));
			AbyssRaidContents = abyssRaidContents;

			ObservableCollection<Content> guildIslandContents = new ObservableCollection<Content>();
			guildIslandContents.Add(new Content("슬라임", true));
			guildIslandContents.Add(new Content("메데이아", true));
			GuildIslandContents = guildIslandContents;

			ObservableCollection<Content> weeklyContents = new ObservableCollection<Content>();
			weeklyContents.Add(new Content("유령선", true));
			WeeklyContents = weeklyContents;

			ObservableCollection<Content> weeklyEtcContents = new ObservableCollection<Content>();
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
