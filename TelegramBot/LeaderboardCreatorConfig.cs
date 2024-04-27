
    public enum AuthSaveMode
    {
        PlayerPrefs,
        PersistentDataPath,
        Unhandled
    }
    
    public class LeaderboardCreatorConfig
    {
        public AuthSaveMode authSaveMode = AuthSaveMode.PlayerPrefs;
        public string fileName = "leaderboard-creator-guid.txt";
        public bool isUpdateLogsEnabled = true;
        
        public string leaderboardsFile = "";
    }
