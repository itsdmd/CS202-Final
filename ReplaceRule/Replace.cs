using Contract;

namespace ReplaceRule
{
	public class Replace : IRule
	{
		public string Name => "Replace";

		public string Config => $"{From} => {To}";

		private string From = "from";
		private string To = "to";
		public string Parse
		{
			set
			{
				// From takes the second token until the word before '=>'
				From = string.Join(" ", value.Split(' ').Skip(1).TakeWhile(x => x != "=>").ToArray());

				// To takes the tokens after '=>'
				To = string.Join(" ", value.Split(' ').SkipWhile(x => x != "=>").Skip(1).ToArray());
			}
		}

		public string Rename(string origin, int fileIndex)
		{
			return (origin.Replace(From, To));
		}

		public object Clone() => MemberwiseClone();
	}
}