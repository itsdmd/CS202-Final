using Contract;

namespace AddPrefixRule
{
	public class AddPrefix : IRule
	{
		public string Name => "AddPrefix";
		
		public string Config => Prefix;
		
		private string Prefix = "prefix";
		
		public string Parse
		{
			set
			{
				// Remove the first word from the string and take the rest as the prefix
				var words = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1);
				Prefix = string.Join(" ", words);
			}
		}

		public string Rename(string inp, int fileIndex)
		{
			return (Prefix + " " + inp);
		}

		public object Clone() => MemberwiseClone();
	}
}