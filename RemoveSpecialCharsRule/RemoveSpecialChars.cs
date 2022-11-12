using Contract;
using System.Text;

namespace RemoveSpecialCharsRule
{
	public class RemoveSpecialChars : IRule
	{
		public string Name => "RemoveSpecialChars";

		public string Config
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				
				foreach (var item in BlackList)
				{
					sb.Append(item);
				}

				return sb.ToString();
			}
		}

		public List<char> BlackList { get; set; } = new List<char>() { '-', '_' };
		public string Parse
		{
			set
			{
				// Remove characters before the first ' '
				int index = value.IndexOf(' ');
				if (index > 0)
				{
					value = value.Substring(index + 1);
				}
				
				// Add each character from substring to BlackList
				foreach (var item in value)
				{
					if (!BlackList.Contains(item))
					{
						BlackList.Add(item);
					}
				}
			}
		}



		public string Rename(string origin, int fileIndex)
		{
			var result = new StringBuilder();
			
			foreach (var c in origin)
			{
				if (!BlackList.Contains(c))
				{
					result.Append(c);
				}
				else
				{
					// Replace with space
					result.Append(' ');
				}
			}

			return result.ToString();
		}

		public object Clone() => MemberwiseClone();
	}
}