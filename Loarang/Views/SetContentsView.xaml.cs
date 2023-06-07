using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
	/// SetContentsView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SetContentsView : UserControl
	{
		public SetContentsView()
		{
			InitializeComponent();
		}
		private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
		{
			if (setContentsDataGrid.CurrentColumn is DataGridCheckBoxColumn)
				setContentsDataGrid.BeginEdit();
		}

		private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
		{
			try
			{
				var checkBox = e.EditingElement as CheckBox;

				if (checkBox != null)
					checkBox.IsChecked = !checkBox.IsChecked;
			}

			catch (Exception ex)
			{

			}
		}
	}
}
