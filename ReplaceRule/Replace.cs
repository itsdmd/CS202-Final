using Contract;

namespace ReplaceRule
{
	public class Replace : IRule
	{
		public string Name => "Replace";
		
		private string From = "";
		private string To = "";
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

		public string Rename(string origin)
		{
			return (origin.Replace(From, To));
		}
	}
}