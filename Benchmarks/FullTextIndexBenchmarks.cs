using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Akade.IndexedSet;
using Akade.IndexedSet.Issue103;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net70)]
public class FullTextIndexBenchmarks
{
    public AutocompleteMst[] _document { get; set; }

    public FullTextIndexBenchmarks()
    {
        // Console.WriteLine(Directory.GetCurrentDirectory());
        string path = "../../../../../../../../data/mst.json";
        string contents = File.ReadAllText(path);
        _document = JsonSerializer.Deserialize<AutocompleteMst[]>(contents, Utils.Jso)!;

        Autocomplete.DataMst = _document;
        Autocomplete.BuildIndexedSetMst(_document);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Latin1")]
    public string[] Linq_Latin_1Character()
    {
        var search = "a";
        return Autocomplete.SearchAutocompleteMstOld(search).ToArray();
    }

    [Benchmark]
    [BenchmarkCategory("Latin1")]
    public string[] IndexedSet_Latin_1Character()
    {
        var search = "a";
        return Autocomplete.SearchAutocompleteMstIndexed(search).ToArray();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Latin2")]
    public string[] Linq_Latin_2Character()
    {
        var search = "ar";
        return Autocomplete.SearchAutocompleteMstOld(search).ToArray();
    }

    [Benchmark]
    [BenchmarkCategory("Latin2")]
    public string[] IndexedSet_2Character()
    {
        var search = "ar";
        return Autocomplete.SearchAutocompleteMstIndexed(search).ToArray();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Hiragana1")]
    public string[] Linq_Hiragana_1Character()
    {
        var search = "あ";
        return Autocomplete.SearchAutocompleteMstOld(search).ToArray();
    }

    [Benchmark]
    [BenchmarkCategory("Hiragana1")]
    public string[] IndexedSet_Hiragana_1Character()
    {
        var search = "あ";
        return Autocomplete.SearchAutocompleteMstIndexed(search).ToArray();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Kanji1")]
    public string[] Linq_Kanji_1Character()
    {
        var search = "俺";
        return Autocomplete.SearchAutocompleteMstOld(search).ToArray();
    }

    [Benchmark]
    [BenchmarkCategory("Kanji1")]
    public string[] IndexedSet_Kanji_1Character()
    {
        var search = "俺";
        return Autocomplete.SearchAutocompleteMstIndexed(search).ToArray();
    }
}