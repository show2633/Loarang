using Loarang.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.ViewModels
{
	class StatsVMState
	{
		private static StatsVM statsVM;

		public static StatsVM StatsVM
		{
			get => statsVM;
			set { statsVM = value; }
		}
	}
}
