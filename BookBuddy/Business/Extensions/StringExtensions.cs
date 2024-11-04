using System.Text.RegularExpressions;

namespace BookBuddy.Business.Extensions
{
    public static class StringExtensions
    {
        public static string ExtractJson(this string content)
        {
            var match = Regex.Match(content, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!))\}");
            return match.Success ? match.Value : string.Empty;
        }
        public static string ExtractJsonArray(this string content)
        {
            var match = Regex.Match(content, @"\[(?:[^\[\]]|(?<open>\[)|(?<-open>\]))+(?(open)(?!))\]");
            return match.Success ? match.Value : string.Empty;
        }

        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Regex för att identifiera romerska siffror
            var romanNumeralsRegex = new Regex(@"\bM{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})\b", RegexOptions.IgnoreCase);

            // Konvertera hela strängen till gemener
            var lowerCasedInput = input.ToLower();

            // Konvertera varje första bokstav i varje ord till versal
            var titleCasedInput = Regex.Replace(lowerCasedInput, @"\b\w", match => match.Value.ToUpper());

            // Återställ romerska siffror till versaler
            var result = romanNumeralsRegex.Replace(titleCasedInput, match => match.Value.ToUpper());

            return result;
        }

    }
}
