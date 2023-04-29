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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Loarang.Views
{
	/// <summary>
	/// ContentsView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ContentsSettingView : UserControl
	{
		public ContentsSettingView()
		{
			InitializeComponent();
		}

		private void settingBtn_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
		{
			if (dayContentsDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				dayContentsDataGrid.BeginEdit();
			else if (commanderRaidContentsDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				commanderRaidContentsDataGrid.BeginEdit();
			else if (abyssContentsSettingDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				abyssContentsSettingDataGrid.BeginEdit();
			else if (abyssRaidContentsDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				abyssRaidContentsDataGrid.BeginEdit();
			else if (guildIslandContentsDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				guildIslandContentsDataGrid.BeginEdit();
			else if (weeklyContentsDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				weeklyContentsDataGrid.BeginEdit();
			else if (weeklyEtcContentsDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				weeklyEtcContentsDataGrid.BeginEdit();
		}

		private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
		{
			try
			{
				var checkBox = e.EditingElement as CheckBox;

				if (checkBox != null)
					checkBox.IsChecked = !checkBox.IsChecked;
			}

			catch(Exception ex)
			{

			}
		}
	}
}
