using Loarang.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.ViewModels
{
	class EquipmentVM : ViewModelBase
	{
		ObservableCollection<Equipment> equipments;
		public EquipmentVM()
		{
			equipments = new ObservableCollection<Equipment>();

			BattleInfoNavigation.SearchAlert += SetEquipment;
		}

		private void SetEquipment(object sender, EventArgs e)
		{
			Equipments = BattleInfoShareStore.Equipments;
		}

		public ObservableCollection<Equipment> Equipments
		{
			get => equipments;
			set { equipments = value; OnPropertyChanged(nameof(Equipments)); }
		}
	}
}
