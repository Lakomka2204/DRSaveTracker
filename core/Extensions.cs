using System.Collections.Generic;
using System.IO;

namespace DRSTCore;

public static class Extensions
{
    public static IEnumerable<string> ReadLines(this StreamReader sr)
    {
        string? line;
        while ((line = sr.ReadLine()) is not null)
            yield return line;
    }
}