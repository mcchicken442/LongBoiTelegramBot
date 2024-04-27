using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;


public sealed class LeaderboardCreatorBehaviour
    {
        [Serializable]
        private struct EntryResponse
        {
            public Entry[] entries;
        }
        
        internal static LeaderboardCreatorConfig Config =>
            Resources.Load<LeaderboardCreatorConfig>("LeaderboardCreatorConfig");

        private static string GetError(WebRequest request) =>
            $"{request.responseCode}: {request.downloadHandler.text}";
        
        internal void Authorize(Action<string> callback)
        {
            var loadedGuid = LoadGuid();
            if (!string.IsNullOrEmpty(loadedGuid))
            {
                callback?.Invoke(loadedGuid);
                return;
            }
            
            var request = WebRequest.Get(GetServerURL(Routes.Authorize));
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(null);
                    return;
                }

                var guid = request.downloadHandler.text;
                SaveGuid(guid);
                callback?.Invoke(guid);
            }));
        }
        
        internal void ResetAndAuthorize(Action<string> callback, Action onFinish)
        {
            callback += guid =>
            {
                if (string.IsNullOrEmpty(guid))
                    return;
                onFinish?.Invoke();
            };
            DeleteGuid();
            Authorize(callback);
        }
        
        internal void SendGetRequest(string url, Action<bool> callback, Action<string> errorCallback)
        {
            var request = WebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(false);
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                callback?.Invoke(true);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }
        
        internal void SendGetRequest(string url, Action<int> callback, Action<string> errorCallback)
        {
            var request = WebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(0);
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                callback?.Invoke(int.Parse(request.downloadHandler.text));
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }
        
        internal void SendGetRequest(string url, Action<Entry> callback, Action<string> errorCallback)
        {
            var request = WebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(new Entry());
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                var response = JsonUtility.FromJson<Entry>(request.downloadHandler.text);
                callback?.Invoke(response);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }
        
        internal void SendGetRequest(string url, Action<Entry[]> callback, Action<string> errorCallback)
        {
            var request = WebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(Array.Empty<Entry>());
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                var response = JsonUtility.FromJson<EntryResponse>($"{{\"entries\":{request.downloadHandler.text}}}");
                callback?.Invoke(response.entries);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }
        /*
        internal void SendPostRequest(string url, List<IMultipartFormSection> form, Action<bool> callback = null, Action<string> errorCallback = null)
        {
            var request = WebRequest.Post(url, form);
            StartCoroutine(HandleRequest(request, callback, errorCallback));
        }
        */
        
        private static IEnumerator HandleRequest(WebRequest request, Action<bool> onComplete, Action<string> errorCallback = null)
        {

            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                onComplete.Invoke(false);
                errorCallback?.Invoke(GetError(request));
                request.downloadHandler.Dispose();
                request.Dispose();
                yield break;
            }

            onComplete.Invoke(true);
            request.downloadHandler.Dispose();
            request.Dispose();
        }
        
        private static void HandleError(WebRequest request)
        {
            var message = Enum.GetName(typeof(StatusCode), (StatusCode) request.responseCode);
            message = string.IsNullOrEmpty(message) ? "Unknown" : message.SplitByUppercase();
                
            var downloadHandler = request.downloadHandler;
            var text = downloadHandler.text;
            if (!string.IsNullOrEmpty(text))
                message = $"{message}: {text}";
            LeaderboardCreator.LogError(message);
        }
        
        private static void SaveGuid(string guid)
        {
            switch (Config.authSaveMode)
            {
                case AuthSaveMode.PlayerPrefs:
                    PlayerPrefs.SetString(GUID_KEY, guid);
                    PlayerPrefs.Save();
                    break;
                case AuthSaveMode.PersistentDataPath:
                    var path = System.IO.Path.Combine(Application.persistentDataPath, Config.fileName);
                    if (string.IsNullOrEmpty(path))
                        return;
                    System.IO.File.WriteAllText(path, guid);
                    break;
            }
        }
        
        private static string LoadGuid()
        {
            switch (Config.authSaveMode)
            {
                case AuthSaveMode.PlayerPrefs:
                    return PlayerPrefs.GetString(GUID_KEY, "");
                case AuthSaveMode.PersistentDataPath:
                    var path = System.IO.Path.Combine(Application.persistentDataPath, Config.fileName);
                    return System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path) : "";
                default:
                    return "";
            }
        }

        private static void DeleteGuid()
        {
            switch (Config.authSaveMode)
            {
                case AuthSaveMode.PlayerPrefs:
                    PlayerPrefs.DeleteKey(GUID_KEY);
                    PlayerPrefs.Save();
                    break;
                case AuthSaveMode.PersistentDataPath:
                    var path = System.IO.Path.Combine(Application.persistentDataPath, Config.fileName);
                    if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                        return;
                    System.IO.File.Delete(path);
                    break;
            }
        }
    }