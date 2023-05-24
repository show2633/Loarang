using Loarang.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loarang.ViewModels
{
	public class SkillVM : ViewModelBase
	{
		Skill skill;

		public SkillVM()
		{
			skill = new Skill();
		}

		public string SkillImage 
		{ get => skill.SkillImage;
			set { skill.SkillImage = value; OnPropertyChanged(nameof(SkillImage)); } 
		}
		public string SkillName
		{ get => skill.SkillName;
			set { skill.SkillName = value; OnPropertyChanged(nameof(SkillName)); } 
		}
		public int SkillLevel 
		{ get => skill.SkillLevel;
			set { skill.SkillLevel = value; OnPropertyChanged(nameof(SkillLevel)); }
		}
		public string SelectedTripods 
		{
			get => SelectedTripods;
			set { SelectedTripods = value; OnPropertyChanged(nameof(SelectedTripods)); }
		}
		public string FirstTripodImage 
		{ 
			get => skill.FirstTripodImage; 
			set { skill.FirstTripodImage = value; OnPropertyChanged(nameof(FirstTripodImage)); }
		}
		public string FirstTripodName 
		{ 
			get => skill.FirstTripodName; 
			set { skill.FirstTripodName = value; OnPropertyChanged(nameof(FirstTripodName)); }
		}
		public string SecondTripodImage 
		{ 
			get => skill.SecondTripodImage;
			set { skill.SecondTripodImage = value; OnPropertyChanged(nameof(SecondTripodImage)); } 
		}
		public string SecondTripodName 
		{
			get => skill.SecondTripodName;
			set { skill.SecondTripodName = value; OnPropertyChanged(nameof(SecondTripodName)); }
		}
		public string ThirdTripodImage 
		{
			get => skill.ThirdTripodImage;
			set { skill.ThirdTripodImage = value; OnPropertyChanged(nameof(ThirdTripodImage)); }
		}
		public string ThirdTripodName 
		{ 
			get => skill.ThirdTripodName; 
			set { skill.ThirdTripodName = value; OnPropertyChanged(nameof(ThirdTripodName)); } 
		}
		public string RuneImage 
		{
			get => skill.RuneImage;
			set { skill.RuneImage = value; OnPropertyChanged(nameof(RuneImage)); }
		}
		public string RuneName 
		{
			get => skill.RuneName;
			set { skill.RuneName = value; OnPropertyChanged(nameof(RuneName)); }
		}
	}
}
