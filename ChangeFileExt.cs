using Contract;

namespace ChangeFileExtensionRule
{
    public class ChangeFileExt : IRule
    {
        public string Name => "ChangeFileExtension";

        public string NewExtension = "";
        public string Parse
        {

            set
            {
                var words = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1);
                NewExtension = string.Join(" ", words);
            }
        }

        public string Rename(string origin)
        {
            return Path.ChangeExtension(origin, NewExtension);
        }
    }
}