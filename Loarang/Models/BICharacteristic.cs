using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.Models
{
	public class BICharacteristic
	{
		public int Crit { get; set; } // 치명
		public int Specialization { get; set; } // 특화
		public int Domination { get; set; } // 제압
		public int Swiftness { get; set; } // 신속
		public int Endurance { get; set; } // 인내
		public int Expertise { get; set; } // 숙련
		public int MaxHP { get; set; } // 최대 생명력
		public int AtkPower { get; set; } // 공격력
	}
}
