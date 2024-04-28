


using System;

public class LeaderboardManager
{
    private string[] _entryTextObjects = new string[1];

    public string[] LoadEntries() {
            
            Leaderboards.To428.GetEntries(entries =>
            {
                _entryTextObjects = new string[entries.Length];
                for (int i = 0; i < entries.Length; i++)
                    _entryTextObjects[i] = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
            });
            return _entryTextObjects;
    }
}