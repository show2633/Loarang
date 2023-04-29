using Loarang.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	class StatsSubNavigation : NavigationBase
	{
		public ICommand ShowEquipmentCommand { get; set; }
		private void ShowEquipment(object obj) => CurrentView = new EquipmentVM();

		public StatsSubNavigation()
		{
			ShowEquipmentCommand = new RelayCommand(ShowEquipment);

			CurrentView = new EquipmentVM();
		}
	}
}
