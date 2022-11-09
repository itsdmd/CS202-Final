using Contract;

namespace AddPrefixRule
{
	public class AddPrefix : IRule
	{
		public string Name => "AddPrefix";
		
		private string Prefix = "";
		
		public string Parse
		{
			set
			{
				// Remove the first word from the string and take the rest as the prefix
				var words = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1);
				Prefix = string.Join(" ", words);
			}
		}

		public string Rename(string inp)
		{
			return (Prefix + " " + inp);
		}
	}
}