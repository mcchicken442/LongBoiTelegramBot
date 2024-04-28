using System;

public class LeaderboardManager
{
    public string[] _entryTextObjects = new string[1];

    public void LoadEntries() {

        var searchQuery = new LeaderboardSearchQuery
        {
            Skip = 0,
            Take = 10,
            TimePeriod = TimePeriodType.AllTime
        };

        Leaderboards.To428.GetEntries(searchQuery, EntriesToString);
    }

    public void EntriesToString(Entry[] entries)
        {
            _entryTextObjects = new string[entries.Length];
            for (int i = 0; i<entries.Length; i++)
                _entryTextObjects[i] = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
        }
}