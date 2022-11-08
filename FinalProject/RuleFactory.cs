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
		public Dictionary<string, IRule> GetPluginDict()
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
		private static string _ruleFile = new string("");
		private static string _fileName = new string("");

		public string RuleFilePath { set { _ruleFile = value; } }
		public string FileName { set { _fileName = value; } }

		// Read rule file
		static List<string> RuleReader()
		{
			try
			{
				return (new List<string>(File.ReadAllLines(_ruleFile)));
			}
			catch (Exception e)
			{
				MessageBox.Show("Rule file not found", e.ToString());
				return null;
			}
		}

		// Apply the rules
		public string Parse()
		{
			if (_ruleFile == "")
			{
				return _fileName;
			}

			List<string> rules = RuleReader();
			string renamed = _fileName;

			Dictionary<string, IRule> rulePrototypes = new DLLReader().GetPluginDict();

			foreach (string r in rules)
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
			}
			
			return renamed;
		}
	}
}