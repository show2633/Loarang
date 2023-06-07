using Loarang.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	public class CharAddVM : ViewModelBase
	{
		private const string USERS_FILE_NAME = "users.csv";
		private const string CONTENTS_FILE_NAME = "contents.csv";

		private string charName;

		public ICommand CharAddCommand { get; }
		public ICommand CharDelCommand { get; }
		public CharAddVM()
		{
			CharAddCommand = new RelayCommand(CharAdd);
			CharDelCommand = new RelayCommand(CharDel);
		}

		private void CharAdd(object obj)
		{
			try
			{
				if(AddCharName != string.Empty &&
					AddCharName != null)
				{
					List<string> characters = LoadCharacters();

					if (characters == null || !characters.Contains(AddCharName))
					{
						using (StreamWriter sw = new StreamWriter(USERS_FILE_NAME, true))
						{
							sw.Write("{0},", AddCharName);
						}
					}					
				}
			}

			catch (Exception e)
			{

			}
		}

		private void CharDel(object obj)
		{
			try
			{
				if (DelCharName != string.Empty &&
					DelCharName != null)
				{
					List<List<string>> contentList = new List<List<string>>();
					List<string> characters = LoadCharacters();

					int delIndex = 0;

					foreach (string character in characters)
					{
						if (character == obj.ToString())
						{
							delIndex = characters.IndexOf(character);

							characters.Remove(character);

							break;
						}
					}

					using (StreamWriter sw = new StreamWriter(USERS_FILE_NAME))
					{
						foreach (string character in characters)
						{
							sw.Write("{0},", character);
						}
					}

					using (StreamReader sr = new StreamReader(CONTENTS_FILE_NAME))
					{
						while (!sr.EndOfStream)
						{
							string line = sr.ReadLine();

							if (line[line.Length - 1] == ',')
							{
								line = line.Substring(0, line.Length - 1);
							}

							string[] contentLine = line.Split(',');
							List<string> content = new List<string>();

							foreach (string cont in contentLine)
							{
								content.Add(cont);
							}

							if(content.Count > 3)
							{
								content.RemoveAt(delIndex + 3);
							}

							contentList.Add(content);
						}
					}

					using (StreamWriter sw = new StreamWriter(CONTENTS_FILE_NAME))
					{
						foreach (List<string> content in contentList)
						{
							foreach (string cont in content)
							{
								sw.Write(cont + ",");
							}

							sw.WriteLine();
						}
					}
				}
			}
			catch(Exception e)
			{

			}
		}

		public List<string> LoadCharacters()
		{
			try
			{
				List<string> characters = new List<string>();

				using (StreamReader sr = new StreamReader(USERS_FILE_NAME))
				{
					string line = sr.ReadLine();
					string[] tempArray = null;

					if (line != null)
					{
						if (line[line.Length - 1] == ',')
						{
							line = line.Substring(0, line.Length - 1);
						}

						tempArray = line.Split(',');
					}				
					
					if(tempArray != null)
					{
						foreach (string character in tempArray)
						{
							characters.Add(character);
						}
					}				
				}

				return characters;
			}

			catch(Exception e)
			{
				return null;
			}
		}

		public string AddCharName
		{
			get => charName;
			set { charName = value; OnPropertyChanged(nameof(AddCharName)); }
		}
		public string DelCharName
		{
			get => charName;
			set { charName = value; OnPropertyChanged(nameof(DelCharName)); }
		}
	}
}
