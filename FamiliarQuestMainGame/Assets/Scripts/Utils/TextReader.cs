using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TextReader
{
    public static List<string[]> ReadSets(string filename)
    {
        List<string[]> output = new List<string[]>();
        var lines = GetLines(filename);
        foreach (var line in lines)
        {
            if (line == "" || line == null) continue;
            var parts = line.Split(',');
            output.Add(parts);
        }
        return output;
    }

    public static List<string> ReadItems(string filename) {
        List<string> output = new List<string>();
        var lines = GetLines(filename);
        foreach (var line in lines) if (line != "" && line != null) output.Add(line);
        return output;
    }

    public static string RandomItem(string filename) {
        var items = ReadItems(filename);
        int roll = UnityEngine.Random.Range(0, items.Count);
        return items[roll];
    }

    public static string[] RandomSet(string filename) {
        List<string[]> output = new List<string[]>();
        var lines = GetLines(filename);
        foreach (var line in lines) {
            if (line == "" || line == null) continue;
            var parts = line.Split(',');
            output.Add(parts);
        }
        return output[UnityEngine.Random.Range(0, output.Count)];
    }

    public static string[] GetLines(string filename) {
        TextAsset data = Resources.Load("Text/" + filename) as TextAsset;
        return data.text.Split(new[] { '\r', '\n' });
    }
}
