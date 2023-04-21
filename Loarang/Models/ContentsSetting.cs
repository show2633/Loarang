using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.Models
{
	public class ContentsSetting
	{
		public ObservableCollection<Content> DailyContents { get; set; }
		public ObservableCollection<Content> CommanderRaidContents { get; set; }
		public ObservableCollection<Content> AbyssContents { get; set; }
		public ObservableCollection<Content> AbyssRaidContents { get; set; }
		public ObservableCollection<Content> GuildIslandContents { get; set; }
		public ObservableCollection<Content> WeeklyContents { get; set; }
		public ObservableCollection<Content> WeeklyEtcContents { get; set; }
	}
}
