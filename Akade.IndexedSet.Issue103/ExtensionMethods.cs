using System.Globalization;
using System.Linq;
using System.Text;

namespace Akade.IndexedSet.Issue103;

public static class ExtensionMethods
{
    public static string NormalizeForAutocomplete(this string input)
    {
        return new string(input
                .Trim()
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .Where(y => (char.IsLetterOrDigit(y) || char.IsWhiteSpace(y)))
                .ToArray())
            .Normalize(NormalizationForm.FormC);
    }
}
