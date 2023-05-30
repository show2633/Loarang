using Loarang.Command;
using System;
using System.IO;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	public class CharAddVM : ViewModelBase
	{
		private const string usersFileName = "users.csv";

		private string charName;

		public ICommand CharAddCommand { get; }
		public CharAddVM()
		{
			CharAddCommand = new RelayCommand(CharAdd);
		}

		private void CharAdd(object obj)
		{
			try
			{
				if(CharName != string.Empty &&
					CharName != null)
				{
					using (StreamWriter sw = new StreamWriter(usersFileName, true))
					{
						sw.Write("{0},", CharName);
					}
				}			
			}

			catch (Exception e)
			{

			}
		}

		public string CharName
		{
			get => charName;
			set { charName = value; OnPropertyChanged(nameof(CharName)); }
		}
	}
}
