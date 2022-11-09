using Contract;

namespace AddSuffixRule
{
    public class AddSuffix : IRule
    {
        public string Name => "AddSuffix";

        public string Suffix = "";
        public string Parse
        {

            set
            {
                // Remove the first word from the string and take the rest as the suffix
                var words = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1);
                Suffix = string.Join(" ", words);
            }
        }

        public string Rename(string origin)
        {
            return (origin + " " + Suffix);
        }
    }
}