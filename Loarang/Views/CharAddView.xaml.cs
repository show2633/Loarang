using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Loarang.Views
{
	/// <summary>
	/// CharAddView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class CharAddView : UserControl
	{
		public CharAddView()
		{
			InitializeComponent();
		}
		private void addBtn_Click(object sender, RoutedEventArgs e)
		{
			addTextBox.Text = string.Empty;
		}

		private void delBtn_Click(object sender, RoutedEventArgs e)
		{
			delTextBox.Text = string.Empty;
		}
	}
}
