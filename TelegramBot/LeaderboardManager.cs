

// NOTE: Make sure to include the following namespace wherever you want to access Leaderboard Creator methods
// using Dan.Main;


public class LeaderboardManager
{
    private string[] _entryTextObjects;

    private string[] LoadEntries() {
            
            Debug.Log("getting entries from 428");
            Leaderboards.To428.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                    {t = "";}
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                    _entryTextObjects[i] = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
                return _entryTextObjects;
            });
    }
}