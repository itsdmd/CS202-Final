using Contract;

namespace OneSpaceRule
{
	public class OneSpace : IRule
	{
		public string Name => "OneSpace";

		public string Config => "";

		public string Parse { get; set; }

		public string Rename(string inp)
		{
			return (string.Join(" ", inp.Split(" ", StringSplitOptions.RemoveEmptyEntries)));
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}