using Contract;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Linq;
using System.Text;

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
		private static List<string> _ruleAsStringList = new List<string>();
		private static string _fileName = new string("");
		private static List<IRule> _ruleList = new List<IRule>();

		public string RulesAsString
		{ 
			get
			{
				StringBuilder sb = new StringBuilder();
				
				foreach (var item in _ruleAsStringList)
				{
					sb.Append(item);
					sb.Append("\n");
				}
				
				return sb.ToString();
			}
			
			set { _ruleAsStringList = value.Split('\n').ToList(); }
		}
		
		public List<string> RulesAsStringList { get => _ruleAsStringList; set => _ruleAsStringList = value; }
		
		public List<IRule> RuleList
		{
			get
			{
				_ruleList.Clear();

				Dictionary<string, IRule> rulePrototypes = new DLLReader().GetRuleDict();

				foreach (string r in _ruleAsStringList)
				{
					string ruleName = r.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];
					if (ruleName == null || ruleName == "") { continue; }

					IRule irule = null;

					try
					{
						irule = rulePrototypes[ruleName];
					}
					catch (KeyNotFoundException)
					{
						MessageBox.Show("Rule " + ruleName + " not found!");
						continue;
					}

					irule.Parse = r;
					_ruleList.Add(irule);
				}

				return _ruleList;
			}
		}
		
		public string FileName { set { _fileName = value; } }

		
		
		public string Parse()
		{
			string renamed = _fileName;
			_ruleList.Clear();

			Dictionary<string, IRule> rulePrototypes = new DLLReader().GetRuleDict();

			foreach (string r in _ruleAsStringList)
			{
				string ruleName = r.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];
				if (ruleName == null || ruleName == "") { continue; }

				IRule irule = null;

				try
				{
					irule = rulePrototypes[ruleName];
				}
				catch (KeyNotFoundException)
				{
					MessageBox.Show("Rule " + ruleName + " not found!");
					continue;
				}

				irule.Parse = r;
				renamed = irule.Rename(renamed);

				_ruleList.Add(irule);
			}
			
			return renamed;
		}
	}
}