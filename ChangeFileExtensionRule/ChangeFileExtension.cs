using Contract;

namespace ChangeFileExtensionRule
{
	public class ChangeFileExtension : IRule
	{
		public string Name => "ChangeFileExtension";
		public string Config => NewExtension;

		private string NewExtension = "doc";
		public string Parse
		{
			set
			{
				var words = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1);
				NewExtension = string.Join(" ", words);
			}
		}

		public string Rename(string origin, int fileIndex)
		{
			// Replace substring after the last '.' with NewExtension
			var index = origin.LastIndexOf('.');
			return origin.Remove(index + 1) + NewExtension;
		}

		public object Clone() => MemberwiseClone();
	}
}