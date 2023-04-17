using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.Models
{
	public class Content
	{
		public string ContentName { get; set; }
		public bool ContentFlag { get; set; }
		public Content(string content)
		{
			this.ContentName = content;
		}
		public Content(string content, bool contentFlag)
		{
			this.ContentName = content;
			this.ContentFlag = contentFlag;
		}
	}
}
