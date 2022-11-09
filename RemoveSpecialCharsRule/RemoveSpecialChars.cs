using Contract;
using System.Text;

namespace RemoveSpecialCharsRule
{
	public class RemoveSpecialChars : IRule
	{
		public string Name => "RemoveSpecialChars";

		public List<char> BlackList { get; set; } = new List<char>();
		public string Parse
		{
			set
			{
				// Split string and add each character to BlackList (except for the first token)
				var tokens = value.Split(' ');
				for (int i = 1; i < tokens.Length; i++)
				{
					if (tokens[i].Length == 1)
					{
						BlackList.Add(tokens[i][0]);
					}
					else
					{
						throw new Exception("E: RemoveSpecialCharsRule: Each token should only be 1 character long");
					}
				}
			}
		}



		public string Rename(string origin)
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
	}
}