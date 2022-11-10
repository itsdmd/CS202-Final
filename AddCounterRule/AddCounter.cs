using Contract;
using System.Diagnostics;

namespace AddCounterRule
{
    public class AddCounter : IRule
    {
        public string Name => "AddCounter";

        public string Config => $"start={Start} step={Step} digits={NumOfDigits}";

        private int Start = 0;
        private int Step = 1;
        private int NumOfDigits = 1;
        public string Parse
        {
            set
            {
                // Remove the first word from the string and take the rest as parameters.
                var parameters = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
                foreach (var param in parameters)
                {
                    // Get index of first occurrence of character '='
                    var index = param.LastIndexOf('=');
                    if (index == -1) continue;

                    // Get part of string before character '='. This is the parameter name.
                    string target = param.Substring(0, index);

                    // Get part of string after character '='. That will be the value for our current parameter.
                    if (param.Length > index + 1)
                    {
                        switch (target)
                        {
                            case "start":
                                Start = Convert.ToInt32(param.Substring(index + 1));
                                break;

                            case "step":
                                Step = Convert.ToInt32(param.Substring(index + 1));
                                break;

                            case "digits":
                                NumOfDigits = Convert.ToInt32(param.Substring(index + 1));
                                break;
                        }
                    }

                }
                // Assert lowest limit to variables.
                Start = Math.Max(0, Start);
                Step = Math.Max(1, Step);
                NumOfDigits = Math.Max(1, NumOfDigits);
            }
        }

        public string Rename(string origin, int fileIndex)
        {
            // Add Counter before the last '.' to prevent changing file's extension (Similar behavior to AddSuffix)
            var index = origin.LastIndexOf('.');
            if (index == -1) index = origin.Length;

            int count = Start + fileIndex * Step;
            return origin.Insert(index, ($"{MakeDigit(count, NumOfDigits)}"));
        }

        private string MakeDigit(int number, int digitCount)
        {
            string strNumber = number.ToString();
            int digitsToAdd = Math.Max(0, digitCount - strNumber.Length);
            while (digitsToAdd > 0)
            {
                strNumber = '0' + strNumber;
                digitsToAdd--;
            }

            return strNumber;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}