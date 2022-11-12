using Contract;

namespace RemoveSurroundingSpacesRule
{
	public class RemoveSurroundingSpaces : IRule
	{
		public string Name => "RemoveSurroundingSpaces";

		public string Config => "";

		public string Parse { get; set; }

		public string Rename(string inp, int fileIndex)
		{
			return inp.Trim();
		}

		public object Clone() => MemberwiseClone();
	}
}