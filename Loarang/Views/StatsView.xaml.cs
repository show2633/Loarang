using Loarang.ViewModels;
using System.Windows.Controls;

namespace Loarang.Views
{
	/// <summary>
	/// StatsView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class StatsView : UserControl
	{
		public StatsView()
		{
			InitializeComponent();
			DataContext = StatsVMState.StatsVM;
		}
	}
}
