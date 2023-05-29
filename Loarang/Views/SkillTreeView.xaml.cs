﻿using Loarang.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	/// SkillView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SkillTreeView : UserControl
	{
		public SkillTreeView()
		{
			DataContext = SkillTreeVMState.SkillTreeVM;
			InitializeComponent();
		}
		private void Link_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			var url = e.Uri.ToString();
			Process.Start(new ProcessStartInfo(url)
			{
				UseShellExecute = true
			});
		}
	}
}
