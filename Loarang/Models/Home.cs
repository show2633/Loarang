using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.Models
{
	public class Home
	{
		public ObservableCollection<HomeNotice> HomeNoticeList { get; set; }
		public ObservableCollection<HomeEvent> HomeEventList { get; set; }
		public ObservableCollection<HomeAdventureIsland> HomeAdventureIslandList { get; set; }

		public Home()
		{
			HomeNoticeList = new ObservableCollection<HomeNotice>();
			HomeEventList = new ObservableCollection<HomeEvent>();
			HomeAdventureIslandList = new ObservableCollection<HomeAdventureIsland>();
		}
	}
}
