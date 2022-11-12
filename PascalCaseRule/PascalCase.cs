using Contract;

namespace PascalCaseRule
{
	public class PascalCase : IRule
	{
		public string Name => "PascalCase";

		public string Config => "";

		public string Parse { get; set; }

		public string Rename(string origin, int fileIndex)
		{
			var words = origin.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
			words = words.Select(word => char.ToUpper(word[0]) + word.Substring(1))
						 .ToArray();

			return string.Join(string.Empty, words);
		}

		public object Clone() => MemberwiseClone();
	}
}