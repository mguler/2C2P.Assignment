using System.Linq;
using System.Text.RegularExpressions;

namespace _2c2p.Assignment.Tools.Impl.Extensions
{
    public static class StringExtensions
    {
        public static string[] Matches(this string text, string pattern) => Regex.Matches(text, pattern).Select(match => match.Value).ToArray();
        public static string Match(this string text, string pattern) => Regex.Match(text, pattern).Value;
        public static bool IsMatch(this string text, string pattern) => Regex.IsMatch(text, pattern);
    }
}
