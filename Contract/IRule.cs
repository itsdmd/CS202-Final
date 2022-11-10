namespace Contract
{
	public interface IRule
	{
		string Name { get; }
		string Config { get; }
		string Parse { set; }
		string Rename(string origin);
	}
}