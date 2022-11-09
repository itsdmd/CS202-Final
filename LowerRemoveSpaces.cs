using Contract;

namespace LowercaseAndRemoveAllSpacesRule
{
    public class LowerRemoveSpaces : IRule
    {
        public string Name => "LowercaseAndRemoveAllSpaces";

        public string Parse { get; set; }

        public string Rename(string origin)
        {
            origin = origin.ToLower();
            origin = origin.Replace(" ", "");
            return origin;
        }
    }
}