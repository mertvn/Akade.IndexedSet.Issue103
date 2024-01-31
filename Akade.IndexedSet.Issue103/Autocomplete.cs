using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Akade.IndexedSet;

namespace Akade.IndexedSet.Issue103;

public static class Autocomplete
{
    public static AutocompleteMst[] DataMst { get; set; }

    public static IndexedSet<AutocompleteMst> SetMst { get; set; }

    public static void BuildIndexedSetMst(IEnumerable<AutocompleteMst> data)
    {
        SetMst = data.ToIndexedSet()
            .WithFullTextIndex(x => x.MSTLatinTitle.NormalizeForAutocomplete())
            .WithFullTextIndex(x => x.MSTNonLatinTitle.NormalizeForAutocomplete())
            .Build();
    }

    public static IEnumerable<string> SearchAutocompleteMstOld(string arg)
    {
        arg = arg.NormalizeForAutocomplete();

        var startsWith = DataMst.Where(x =>
                x.MSTLatinTitleNormalized.StartsWith(arg) || x.MSTNonLatinTitleNormalized.StartsWith(arg))
            .OrderBy(x => x.MSTLatinTitle)
            .ToArray();

        var contains = DataMst.Where(x =>
                x.MSTLatinTitleNormalized.Contains(arg) || x.MSTNonLatinTitleNormalized.Contains(arg))
            .OrderBy(x => x.MSTLatinTitle)
            .ToArray();

        var startsWithLT = startsWith.Select(x => x.MSTLatinTitle);
        var startsWithNLT = startsWith.Select(x => x.MSTNonLatinTitle);

        var containsLT = contains.Select(x => x.MSTLatinTitle);
        var containsNLT = contains.Select(x => x.MSTNonLatinTitle);

        string[] final = startsWithLT.Concat(containsLT)
            .Concat(startsWithNLT).Concat(containsNLT)
            .Distinct()
            .Where(x => x != "").ToArray();

        return final.Any() ? final.Take(25) : Array.Empty<string>();
    }

    public static IEnumerable<string> SearchAutocompleteMstIndexed(string arg)
    {
        arg = arg.NormalizeForAutocomplete();

        var startsWith1 = SetMst.StartsWith(x => x.MSTLatinTitle.NormalizeForAutocomplete(), arg);
        var startsWith2 = SetMst.StartsWith(x => x.MSTNonLatinTitle.NormalizeForAutocomplete(), arg);
        var startsWith = startsWith1.Concat(startsWith2).OrderBy(x => x.MSTLatinTitle).ToArray();

        var contains1 = SetMst.Contains(x => x.MSTLatinTitle.NormalizeForAutocomplete(), arg);
        var contains2 = SetMst.Contains(x => x.MSTNonLatinTitle.NormalizeForAutocomplete(), arg);
        var contains = contains1.Concat(contains2).OrderBy(x => x.MSTLatinTitle).ToArray();

        var startsWithLT = startsWith.Select(x => x.MSTLatinTitle);
        var startsWithNLT = startsWith.Select(x => x.MSTNonLatinTitle);

        var containsLT = contains.Select(x => x.MSTLatinTitle);
        var containsNLT = contains.Select(x => x.MSTNonLatinTitle);

        string[] final = startsWithLT.Concat(containsLT)
            .Concat(startsWithNLT).Concat(containsNLT)
            .Distinct()
            .Where(x => x != "").ToArray();

        return final.Any() ? final.Take(25) : Array.Empty<string>();
    }
}