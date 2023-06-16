using Loarang.Command;
using Loarang.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Loarang.ViewModels
{
	public class SetContentsVM : ViewModelBase
	{
		private const string CONTENTS_FILE_NAME = "contents.csv";
		private const string USERS_FILE_NAME = "users.csv";

		DataTable setContentsDT;

		public ICommand SaveSetContentsCommand { get; }
		public ICommand LoadSetContentsCommand { get; }

		public SetContentsVM()
		{
			setContentsDT = new DataTable();
			SetContentsDT = CreateContentsDT();

			SaveSetContentsCommand = new RelayCommand(SaveSetContents);
			LoadSetContentsCommand = new RelayCommand(LoadSetContents);
		}
		private void SaveSetContents(object obj)
		{
			try
			{
				if (SetContentsDT == null)
				{
					return;
				}

				List<string[]> contentList = new List<string[]>();

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

						contentList.Add(contentLine);
					}
				}

				using (StreamWriter sw = new StreamWriter(CONTENTS_FILE_NAME))
				{
					List<List<string>> setContentList = new List<List<string>>();

					foreach (DataRow row in SetContentsDT.Rows)
					{
						List<string> setContent = new List<string>();

						for (int i = 0; i < SetContentsDT.Columns.Count; i++)
						{
							setContent.Add(row[i].ToString());
						}

						setContentList.Add(setContent);
					}

					contentList[0].Contains("");

					for (int i = 0; i < contentList.Count; i++)
					{
						for (int j = 0; j < setContentList.Count; j++)
						{
							if (contentList[i][1] == setContentList[j][0])
							{
								for (int k = 0; k < setContentList[j].Count - 1; k++)
								{
									contentList[i][k + 3] = setContentList[j][k + 1];
								}								
							}
						}
					}
				}

				using (StreamWriter sw_2 = new StreamWriter(CONTENTS_FILE_NAME))
				{
					foreach (string[] conts in contentList)
					{
						foreach (string cont in conts)
						{
							sw_2.Write(cont + ",");
						}

						sw_2.WriteLine();
					}
				}
			}

			catch (Exception e)
			{

			}
		}

		private void LoadSetContents(object obj)
		{
			DataTable tempDT = new DataTable();

			try
			{
				string[] characters = null;

				using (StreamReader sr = new StreamReader(USERS_FILE_NAME))
				{
					string line = sr.ReadLine();

					if (line != null)
					{
						if (line[line.Length - 1] == ',')
						{
							line = line.Substring(0, line.Length - 1);
						}

						characters = line.Split(',');
					}

					else
					{
						return;
					}
				}

				DataColumn dc = new DataColumn("컨텐츠", typeof(string));
				dc.ReadOnly = true;

				tempDT.Columns.Add(dc);

				foreach (string character in characters)
				{
					tempDT.Columns.Add(character, typeof(bool));
				}

				using (StreamReader sr = new StreamReader(CONTENTS_FILE_NAME))
				{
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine();
						line = line.Substring(0, line.Length - 1);

						string[] contents = line.Split(',');

						DataRow dr = tempDT.NewRow();

						dr["컨텐츠"] = contents[1];

						for (int i = 0; i < characters.Length; i++)
						{
							dr[$"{characters[i]}"] = contents[i + 3];
						}

						tempDT.Rows.Add(dr);
					}
				}

				if (CompareDataTable(tempDT))
				{
					SetContentsDT = tempDT;
				}

				else
				{
					return;
				}
			}

			catch (Exception e)
			{

			}
		}

		private bool CompareDataTable(DataTable dt)
		{
			if (SetContentsDT.Rows.Count != dt.Rows.Count || SetContentsDT.Columns.Count != dt.Columns.Count)
				return false;


			for (int i = 0; i < SetContentsDT.Rows.Count; i++)
			{
				for (int c = 0; c < SetContentsDT.Columns.Count; c++)
				{
					if (!Equals(SetContentsDT.Rows[i][c], dt.Rows[i][c]))
						return false;
				}
			}
			return true;
		}

		private DataTable CreateContentsDT()
		{
			DataTable tempDT = new DataTable();

			try
			{
				setContentsDT.Columns.Clear();
				setContentsDT.Rows.Clear();

				string[] characters = null;

				ObservableCollection<Content> contents = new ObservableCollection<Content>();
				List<string[]> contentList = new List<string[]>();

				using (StreamReader sr = new StreamReader(USERS_FILE_NAME))
				{
					string line = sr.ReadLine();

					if(line != null)
					{
						if (line[line.Length - 1] == ',')
						{
							line = line.Substring(0, line.Length - 1);
						}

						characters = line.Split(',');
					}

					else
					{
						return null;
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

						contentList.Add(contentLine);
						//Content content = new Content(contentLine[1], bool.Parse(contentLine[2]));

						//contents.Add(content);
					}
				}

				DataColumn dc = new DataColumn("컨텐츠", typeof(string));
				dc.ReadOnly = true;

				tempDT.Columns.Add(dc);

				foreach (string character in characters)
				{
					tempDT.Columns.Add(character, typeof(bool));
				}
				//foreach(string[] conts in contentList)
				for (int i = 0; i < contentList.Count; i++)
				{
					if (contentList[i][2] == "True")
					{
						DataRow dr = tempDT.NewRow();

						dr["컨텐츠"] = contentList[i][1];

						if (contentList[i].Length - 3 != characters.Length)
						{
							bool flag = false;

							int sub = characters.Length - (contentList[i].Length - 3);

							for (int j = 0; j < sub; j++)
							{
								dr[$"{characters[j]}"] = flag;
								contentList[i] = contentList[i].Append("False").ToArray();
							}
						}

						else
						{
							for (int j = 0; j < characters.Length; j++)
							{
								dr[$"{characters[j]}"] = contentList[i][j + 3];
							}
						}

						tempDT.Rows.Add(dr);
					}
				}

				using (StreamWriter sw = new StreamWriter(CONTENTS_FILE_NAME))
				{
					foreach (string[] conts in contentList)
					{
						foreach (string cont in conts)
						{
							sw.Write(cont + ",");
						}

						sw.WriteLine();
					}
				}

				return tempDT;
			}

			catch (Exception e)
			{
				return tempDT;
			}
		}

		public DataTable SetContentsDT
		{
			get => setContentsDT;
			set { setContentsDT = value; OnPropertyChanged(nameof(SetContentsDT)); }
		}
	}
}
