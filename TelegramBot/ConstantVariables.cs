
    public static class ConstantVariables
    {
        internal const string GUID_KEY = "LEADERBOARD_CREATOR___LOCAL_GUID";
        
        internal static string GetServerURL(Routes route = Routes.None, string extra = "")
        {
            string fullURL = "";
            fullURL += SERVER_URL;
            switch(route) {
                case Routes.Authorize:
                    fullURL += "/authorize";
                    break;
                case Routes.Get:
                    fullURL += "/get";
                    break;
                case Routes.Upload:
                    fullURL += "/entry/upload";
                    break;
                case Routes.UpdateUsername:
                    fullURL += "/entry/update-username";
                    break;
                case Routes.DeleteEntry:
                    fullURL += "/entry/delete";
                    break;
                case Routes.GetPersonalEntry:
                    fullURL += "/entry/get";
                    break;
                case Routes.GetEntryCount:
                    fullURL += "/entry/count";
                    break;
                default:
                    // code block
                    break;
            }
            fullURL += extra;
            return fullURL;
            /*
            return SERVER_URL + route switch
            {
                Routes.Authorize => "/authorize",
                Routes.Get => "/get",
                Routes.Upload => "/entry/upload",
                Routes.UpdateUsername => "/entry/update-username",
                Routes.DeleteEntry => "/entry/delete",
                Routes.GetPersonalEntry => "/entry/get",
                Routes.GetEntryCount => "/entry/count",
                _ => "/"
            } + extra;
            */
        }

        private const string SERVER_URL = "https://lcv2-server.danqzq.games";
    }
