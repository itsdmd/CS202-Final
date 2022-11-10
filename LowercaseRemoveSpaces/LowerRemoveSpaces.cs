using Contract;

namespace LowercaseAndRemoveAllSpacesRule
{
    public class LowerRemoveSpaces : IRule
    {
        public string Name => "LowerRemoveSpaces";

        public string Parse { get; set; }

        public string Config => "";

        public string Rename(string origin)
        {
            origin = origin.ToLower();
            origin = origin.Replace(" ", "");
            return origin;
        }
    }
}