using Contract;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace FinalProject
{
	public class DLLReader
	{
		public Dictionary<string, IRule> GetRuleDict()
		{
			string folder = AppDomain.CurrentDomain.BaseDirectory;
			DirectoryInfo folderInfo = new DirectoryInfo(folder);
			var dllFiles = folderInfo.GetFiles("*.dll");

			Dictionary<string, IRule> dict = new Dictionary<string, IRule>();

			foreach (var file in dllFiles)
			{
				var assembly = Assembly.LoadFrom(file.FullName);

				var types = assembly.GetTypes();

				foreach (var type in types)
				{
					if (type.IsClass
						&& typeof(IRule).IsAssignableFrom(type))
					{
						dict.Add(type.Name.Replace("Rule", ""),
						(IRule)Activator.CreateInstance(type));
					}
				}
			}

			return dict;
		}
	}

	public class RuleFactory
	{
		private string _fileName = new string("");
		private int _fileIndex = 0;

        private List<IRule> _ruleList = new List<IRule>();
		private Dictionary<string, IRule> rulePrototypes = new DLLReader().GetRuleDict();

		public IRule StringToIRuleConverter(string inputStr)
		{
			try
			{
				IRule newIRule = rulePrototypes[inputStr.Split(' ')[0]].Clone() as IRule;

				newIRule.Parse = inputStr;
				return newIRule;
			}
			catch (KeyNotFoundException)
			{
				MessageBox.Show("Rule " + inputStr.Split(' ')[0] + " not found!");
				return null;
			}
		}
		
		public List<IRule> RuleList
		{
			get => _ruleList;

			set => _ruleList = value;
		}
		
		public void ClearRuleList() => _ruleList.Clear();



		public string FileName { set { _fileName = value; } }
		public int FileIndex { set { _fileIndex = value; } }

		public string Parse()
		{
			if (_ruleList.Count == 0) { return _fileName; }
			
			else
			{
				string renamed = _fileName;

				foreach (IRule irule in _ruleList)
				{
					renamed = irule.Rename(renamed, _fileIndex);
				}

				return renamed;
			}
		}
	}
}