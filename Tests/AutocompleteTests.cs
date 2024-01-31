using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using NUnit.Framework;
using Akade.IndexedSet.Issue103;

namespace Tests;

public class AutocompleteTests
{
    public AutocompleteMst[] Data { get; set; } = Array.Empty<AutocompleteMst>();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Console.WriteLine(Directory.GetCurrentDirectory());
        string path = "../../../../data/mst.json";
        string contents = File.ReadAllText(path);
        Data = JsonSerializer.Deserialize<AutocompleteMst[]>(contents, Utils.Jso)!;
        Autocomplete.DataMst = Data;
        Autocomplete.BuildIndexedSetMst(Data);
    }

    [Test]
    public void Test_IndexedAndOldReturnTheSameResult_Latin()
    {
        const string search = "a";

        var old = Autocomplete.SearchAutocompleteMstOld(search);
        var indexed = Autocomplete.SearchAutocompleteMstIndexed(search);

        string serializedOld = JsonSerializer.Serialize(old, Utils.Jso);
        // Console.WriteLine(serializedOld);

        string serializedIndexed = JsonSerializer.Serialize(indexed, Utils.Jso);
        // Console.WriteLine(serializedIndexed);

        Assert.AreEqual(serializedOld, serializedIndexed);
    }

    [Test]
    public void Test_IndexedAndOldReturnTheSameResult_Kanji()
    {
        const string search = "俺";

        var old = Autocomplete.SearchAutocompleteMstOld(search);
        var indexed = Autocomplete.SearchAutocompleteMstIndexed(search);

        string serializedOld = JsonSerializer.Serialize(old, Utils.Jso);
        // Console.WriteLine(serializedOld);

        string serializedIndexed = JsonSerializer.Serialize(indexed, Utils.Jso);
        // Console.WriteLine(serializedIndexed);

        Assert.AreEqual(serializedOld, serializedIndexed);
    }
}