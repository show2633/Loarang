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
		ObservableCollection<Equipment> accessories;
		ObservableCollection<EtcEquipment> etcEquipments;
		ObservableCollection<BIEngrave> mountingEngraves;
		public EquipmentVM()
		{
			equipments = new ObservableCollection<Equipment>();
			accessories = new ObservableCollection<Equipment>();
			etcEquipments = new ObservableCollection<EtcEquipment>();
			mountingEngraves = new ObservableCollection<BIEngrave>();

			BattleInfoNavigation.SearchAlert += SetEquipment;
		}

		private void SetEquipment(object sender, EventArgs e)
		{
			Equipments = BattleInfoShareStore.Equipments;
			Accessories = BattleInfoShareStore.Accessories;
			EtcEquipments = BattleInfoShareStore.EtcEquipments;
			MountingEngraves = BattleInfoShareStore.MountingEngraves;
		}

		public ObservableCollection<Equipment> Equipments
		{
			get => equipments;
			set { equipments = value; OnPropertyChanged(nameof(Equipments)); }
		}

		public ObservableCollection<Equipment> Accessories
		{
			get => accessories;
			set { accessories = value; OnPropertyChanged(nameof(Accessories)); }
		}

		public ObservableCollection<EtcEquipment> EtcEquipments
		{
			get => etcEquipments;
			set { etcEquipments = value; OnPropertyChanged(nameof(EtcEquipments)); }
		}

		public ObservableCollection<BIEngrave> MountingEngraves
		{
			get => mountingEngraves;
			set { mountingEngraves = value; OnPropertyChanged(nameof(MountingEngraves)); }
		}
	}
}
