﻿using Loarang.Command;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class SubNavigationVM : NavigationBase
	{
		public ICommand ContentsSettingCommand { get; set; }
		public ICommand CharAddCommand { get; set; }
		private void ContentsSetting(object obj) => CurrentView = new ContentsSettingVM();
		private void CharAdd(object obj) => CurrentView = new CharAddVM();
		public SubNavigationVM()
		{
			ContentsSettingCommand = new RelayCommand(ContentsSetting);
			CharAddCommand = new RelayCommand(CharAdd);

			CurrentView = new ContentsSettingVM();
		}
	}
}
