using Loarang.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.ViewModels
{
	class EquipmentVMState
	{
		private static EquipmentVM equipmentVM;
		public static EquipmentVM EquipmentVM
		{
			get => equipmentVM;
			set { equipmentVM = value; }
		}
	}
}
