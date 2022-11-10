using Contract;
using System.Text;

namespace AddSuffixRule
{
    public class AddSuffix : IRule
    {
        public string Name => "AddSuffix";
        public string Config => Suffix;

        private string Suffix = "suffix";
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
            var sb = new StringBuilder();
            int i = origin.IndexOf('.');
            sb.Append(origin);
            sb.Insert(i, Suffix);
            return sb.ToString();

        }
    }
}