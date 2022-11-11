namespace Contract
{
	public interface IRule
	{
        string Name { get; }
        string Config { get; }
        string Parse { set; }
		string Rename(string origin, int fileIndex);
		object Clone();
	}

    // JSON Deserialize Template
    public readonly struct RuleDeserializeTemplate : IRule
	{
		public string Name { get; init; }
		public string Config { get; init; }

        public string Parse { set { } }

        public string Rename(string origin, int fileIndex)
        {
			return origin;
        }

        public object Clone()
		{
			return MemberwiseClone();
		}
	}
}