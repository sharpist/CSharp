using System;

namespace BenchmarkSpan
{
    public class NameParser
    {
        // Using Substring
        public string GetLastNameUsingSubstring(string fullName)
        {
            var lastSpaceIndex = fullName
                .LastIndexOf(" ", StringComparison.Ordinal);

            return lastSpaceIndex == -1
                ? string.Empty
                : fullName.Substring(lastSpaceIndex + 1);
        }

        // Using Span<T>
        public ReadOnlySpan<char> GetLastNameUsingSpan(ReadOnlySpan<char> fullName)
        {
            var lastSpaceIndex = fullName.LastIndexOf(' ');

            return lastSpaceIndex == -1
                ? ReadOnlySpan<char>.Empty
                : fullName.Slice(lastSpaceIndex + 1);
        }
    }
}
