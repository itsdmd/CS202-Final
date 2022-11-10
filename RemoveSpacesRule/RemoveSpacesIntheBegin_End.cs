using Contract;

namespace RemoveSpacesRule
{
    public class RemoveSpacesIntheBegin_End : IRule
    {
        public string Name => "RemoveSpacesIntheBegin_End";

        public string Config => "";

        public string Parse { get; set; }

        public string Rename(string inp)
        {
            return inp.Trim();
        }
    }
}