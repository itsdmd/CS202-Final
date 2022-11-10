using Contract;

namespace ChangeFileExtensionRule
{
    public class ChangeFileExtension : IRule
    {
        public string Name => "ChangeFileExtension";

        private string NewExtension = "doc";
        public string Config => NewExtension;
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