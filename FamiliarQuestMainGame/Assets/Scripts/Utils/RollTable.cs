using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TableRoller
{
    public static string Roll(string table)
    {
        var items = TextReader.ReadSets("Tables/" + table);
        List<RollEntry> entries = new List<RollEntry>();
        foreach (var item in items) entries.Add(new RollEntry(int.Parse(item[0]), item[1]));
        for (int i = 0; i < entries.Count; i++)
        {
            if (i == 0) entries[i].Pad();
            else entries[i].Pad(entries[i - 1]);
        }
        int roll = Random.Range(0,entries[entries.Count-1].rollUnder);
        foreach (var entry in entries) if (roll < entry.rollUnder) return entry.value;
        return entries[entries.Count - 1].value;
    }
}

public class RollEntry
{
    public int chance;
    public int rollUnder;
    public string value;
    public RollEntry(int chance, string value)
    {
        this.chance = chance;
        this.value = value;
    }
    public void Pad(RollEntry lastEntry=null)
    {
        if (lastEntry == null) rollUnder = chance;
        else rollUnder = lastEntry.rollUnder + chance;
    }
}