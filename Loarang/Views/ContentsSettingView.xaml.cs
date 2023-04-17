using Loarang.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Loarang.Views
{
	/// <summary>
	/// ContentsSettingView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ContentsSettingView : Window
	{
		public ContentsSettingView()
		{
			InitializeComponent();
			testSetting();
		}

		[DllImport("user32.dll")]

		public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lPanam);

		private void ControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			WindowInteropHelper helper = new WindowInteropHelper(this);
			SendMessage(helper.Handle, 161, 2, 0);
		}

		private void ControlBar_MouseEnter(object sender, MouseEventArgs e)
		{
			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; // 마우스가 컨트롤바에 갈 때마다 최대높이 갱신
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void btnMaximize_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Normal)
				this.WindowState = WindowState.Maximized;
			else this.WindowState = WindowState.Normal;
		}

		private void btnMinimize_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void settingBtn_Click(object sender, RoutedEventArgs e)
		{

		}
		private void testSetting()
		{
			ObservableCollection<Content> dailyContents = new ObservableCollection<Content>();
			dailyContents.Add(new Content("일일 에포나", true));
			dailyContents.Add(new Content("카오스 던전", true));
			dailyContents.Add(new Content("가디언 토벌", true));
			dailyContents.Add(new Content("모험 섬", true));
			dailyContents.Add(new Content("필드 보스", true));
			dailyContents.Add(new Content("카오스 게이트", true));
			dailyContents.Add(new Content("이벤트 섬", true));
			this.dayContentsDataGrid.ItemsSource = dailyContents;

			ObservableCollection<Content> commanderRaidContents = new ObservableCollection<Content>();
			commanderRaidContents.Add(new Content("발탄", true));
			commanderRaidContents.Add(new Content("비아키스", true));
			commanderRaidContents.Add(new Content("쿠크세이튼", true));
			commanderRaidContents.Add(new Content("아브렐슈드", true));
			commanderRaidContents.Add(new Content("일리아칸", true));
			this.CommanderRaidContentsDataGrid.ItemsSource = commanderRaidContents;

			ObservableCollection<Content>  abyssContents = new ObservableCollection<Content>();
			abyssContents.Add(new Content("기본 6종", true));
			abyssContents.Add(new Content("낙원 3종", true));
			abyssContents.Add(new Content("오레하", true));
			abyssContents.Add(new Content("카양겔", true));
			abyssContents.Add(new Content("상아탑", true));			
			this.abyssContentsSettingDataGrid.ItemsSource = abyssContents;

			ObservableCollection<Content> abyssRaidContents = new ObservableCollection<Content>();
			abyssRaidContents.Add(new Content("아르고스", true));
			this.abyssRaidContentsDataGrid.ItemsSource = abyssRaidContents;

			ObservableCollection<Content> guildIslandContents = new ObservableCollection<Content>();
			guildIslandContents.Add(new Content("슬라임", true));
			guildIslandContents.Add(new Content("메데이아", true));
			this.guildIslandContentsDataGrid.ItemsSource = guildIslandContents;

			ObservableCollection<Content> weeklyContents = new ObservableCollection<Content>();
			weeklyContents.Add(new Content("유령선", true));
			this.weeklyContentsDataGrid.ItemsSource = weeklyContents;

			ObservableCollection<Content> weeklyEtcContents = new ObservableCollection<Content>();
			weeklyEtcContents.Add(new Content("엘가시아 보석 교환", true));
			weeklyEtcContents.Add(new Content("페이토 카드 교환", true));
			weeklyEtcContents.Add(new Content("페르마타 카드 교환", true));
			weeklyEtcContents.Add(new Content("혈석 상자 교환", true));
			weeklyEtcContents.Add(new Content("길드 의뢰", true));
			weeklyEtcContents.Add(new Content("주간 에포나", true));
			weeklyEtcContents.Add(new Content("길드 토벌", true));
			weeklyEtcContents.Add(new Content("이벤트 상점 교환", true));
			this.weeklyEtcContentsDataGrid.ItemsSource = weeklyEtcContents;

			//collection.Add(new Content("[기타]"));
			//collection.Add(new Content("큐브")); // 추후 수정

			
		}
	}
}
