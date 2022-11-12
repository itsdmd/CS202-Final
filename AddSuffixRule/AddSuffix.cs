using Contract;

namespace AddSuffixRule
{
	public class AddSuffix : IRule
	{
		public string Name => "AddSuffix";
		
		public string Config => Suffix;

		public string Suffix = "suffix";
		public string Parse
		{
			set
			{
				// Remove the first word from the string and take the rest as the suffix
				var words = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1);
				Suffix = string.Join(" ", words);
			}
		}

		public string Rename(string origin, int fileIndex)
		{
			// Add Suffix before the last '.' to prevent changing file's extension
			var index = origin.LastIndexOf('.');
			return origin.Insert(index, (" " + Suffix));
		}

		public object Clone() => MemberwiseClone();
	}
}