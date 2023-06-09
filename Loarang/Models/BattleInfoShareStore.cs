﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.Models
{
	public static class BattleInfoShareStore
	{
		public static BIProfiles BIProfiles { get; set; }
		public static ObservableCollection<BIJewel> BIJewels { get; set; }
		public static BICharacteristic BICharacteristic { get; set; }
		public static ObservableCollection<BIEngrave> BIEngraves { get; set; }
		public static ObservableCollection<BICardImage> BICardImages { get; set; }
		public static ObservableCollection<BICardDescription> BICardDescriptions { get; set; }
		public static ObservableCollection<Equipment> Equipments { get; set; }
		public static ObservableCollection<BIEngrave> MountingEngraves { get; set; }
		public static ObservableCollection<Equipment> Accessories { get; set; }
		public static ObservableCollection<EtcEquipment> EtcEquipments { get; set; }
		public static ObservableCollection<Skill> Skills { get; set; }
	}
}
