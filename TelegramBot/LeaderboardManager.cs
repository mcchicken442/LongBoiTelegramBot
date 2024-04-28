


using System;

public class LeaderboardManager
{
    private string[] _entryTextObjects = new string[1];

    public string[] LoadEntries() {

        var searchQuery = new LeaderboardSearchQuery
        {
            Skip = 0,
            Take = 100,
            TimePeriod = TimePeriodType.AllTime
        };

        Leaderboards.To428.GetEntries(searchQuery, EntriesToString);
            
        return _entryTextObjects;
    }

    public void EntriesToString(Entry[] entries)
        {
            _entryTextObjects = new string[entries.Length];
            for (int i = 0; i<entries.Length; i++)
                _entryTextObjects[i] = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
        }
}