using SimpleStepParser.SimplifiedModelRepresentation.Domain;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleStepParser.SimplifiedModelRepresentation.Application;

internal static class SiemensNxModelNameFixer
{
    public static Model? FixName(Model? model)
    {
        if (model == null)
        {
            return null;
        }
        Model result = model.GetDeepCopy();
        FixModelNameRecursive(result);
        return result;
    }

    private static void FixModelNameRecursive(Model model)
    {
        if (model == null)
        {
            return;
        }

        string newName = FixString(model.Name ?? string.Empty);

        model.Name = newName;

        foreach (Model child in model.Childs)
        {
            FixModelNameRecursive(child);
        }
    }

    private static Regex _ackiiRegionRegex = new Regex(@"(?<ascii>\\X2\\[0-9A-Fa-f]*\\X0\\)", RegexOptions.Compiled);
    private static Regex _ackiiRegionRegex2 = new Regex("(?<ascii>_X2_[0-9A-Fa-f]*_X0_)", RegexOptions.Compiled);
    private static Regex _ackiiRegionRegex3 = new Regex("(?<ascii>_X2_[0-9A-Fa-f]*_)", RegexOptions.Compiled);
    private static Regex _ackiiRegionRegex4 = new Regex(@"(?<ascii>\\\\X2\\\\[0-9A-Fa-f]*\\\\X0\\\\)", RegexOptions.Compiled);

    public static string FixString(string name)
    {
        string result = name.Replace(Environment.NewLine, "");

        //Fixing expressions like /X2/123412341234/X0/
        MatchCollection matches = _ackiiRegionRegex.Matches(result);
        if (matches.Count > 0)
        {
            foreach (Match? match in matches.ToList())
            {
                string asciiString =
                    match.Value.Substring(4, match.Value.Length - 8);
                string fixedString = AsciiToString(asciiString);
                result = result.Replace(match.Value, fixedString);
            }
        }

        //Fixing expressions like _X2_123412341234_X0_
        matches = _ackiiRegionRegex2.Matches(result);
        if (matches.Count > 0)
        {
            foreach (Match? match in matches.ToList())
            {
                string asciiString =
                    match.Value.Substring(4, match.Value.Length - 8);
                string fixedString = AsciiToString(asciiString);
                result = result.Replace(match.Value, fixedString);
            }
        }

        //Fixing expressions like _X2_123412341234_
        matches = _ackiiRegionRegex3.Matches(result);
        if (matches.Count > 0)
        {
            foreach (Match? match in matches.ToList())
            {
                string asciiString =
                    match.Value.Substring(4, match.Value.Length - 5);
                string fixedString = AsciiToString(asciiString);
                result = result.Replace(match.Value, fixedString);
            }
        }

        //Fixing expressions like //X2//123412341234//X0//
        matches = _ackiiRegionRegex3.Matches(result);
        if (matches.Count > 0)
        {
            foreach (Match? match in matches.ToList())
            {
                string asciiString =
                    match.Value.Substring(6, match.Value.Length - 12);
                string fixedString = AsciiToString(asciiString);
                result = result.Replace(match.Value, fixedString);
            }
        }

        return result;
    }


    private static string AsciiToString(string ascii)
    {
        StringBuilder result = new();
        for (int i = 0; i < ascii.Length / 4; i++)
        {
            string asciiNumberString = ascii.Substring(i * 4, 4);
            int askiiNumber = 0;
            if (int.TryParse(asciiNumberString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out askiiNumber))
            {
                result.Append((char)askiiNumber);
            }
        }
        return result.ToString();
    }
}
