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
			ObservableCollection<Content> collection = new ObservableCollection<Content>();

			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content(""));
			collection.Add(new Content("[군단장]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content(""));
			collection.Add(new Content("[어비스 던전]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content(""));
			collection.Add(new Content("[어비스 레이드]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content(""));
			collection.Add(new Content("[점령 섬]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content(""));
			collection.Add(new Content("[주간 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content(""));
			collection.Add(new Content("[주간 기타]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content("[일일 컨텐츠]"));
			collection.Add(new Content(""));
			collection.Add(new Content("[기타]"));
			collection.Add(new Content("[일일 컨텐츠]"));
		}
	}
}
