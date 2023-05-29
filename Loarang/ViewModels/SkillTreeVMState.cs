using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.ViewModels
{
	class SkillTreeVMState
	{
		private static SkillTreeVM skillTreeVM;

		public static SkillTreeVM SkillTreeVM
		{
			get => skillTreeVM;
			set { skillTreeVM = value; }
		}
	}
}
