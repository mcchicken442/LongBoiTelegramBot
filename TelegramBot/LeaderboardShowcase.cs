using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;


    public class LeaderboardShowcase
    {
        private int _defaultPageNumber = 1, _defaultEntriesToTake = 100;
        private Entry[] leaderboardEntries;
        
        public void Load()
        {
            var timePeriod = 
                //Dan.Enums.TimePeriodType.Today :
                //Dan.Enums.TimePeriodType.ThisWeek :
                //Dan.Enums.TimePeriodType.ThisMonth :
                //Dan.Enums.TimePeriodType.ThisYear :
                Dan.Enums.TimePeriodType.AllTime;

            var pageNumber = _defaultPageNumber;
            pageNumber = Mathf.Max(1, pageNumber);
            _pageInput.text = pageNumber.ToString();
            
            var take = _defaultEntriesToTake;
            take = Mathf.Clamp(take, 1, 100);
            
            var searchQuery = new LeaderboardSearchQuery
            {
                Skip = (pageNumber - 1) * take,
                Take = take,
                TimePeriod = timePeriod
            };
            
            Leaderboards.To428.GetEntries(searchQuery, OnLeaderboardLoaded, ErrorCallback);
        }

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            leaderboardEntries = entries;

            foreach (var t in entries) {
                Debug.Log(t.Username);
                //Debug.Log(t.PublicKey);
                //Debug.Log(t.UserGuid);
                Debug.Log(t.Score);
                //LeaderboardCreator.DeleteEntry(t.publicKey);
                foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(t))
                {
                    string name = descriptor.Name;
                    //object value = descriptor.GetValue(obj);
                    Debug.Log(name);
                }
            }
        }
        
        private void Callback(bool success)
        {
            if (success)
                Load();
        }
        
        private void ErrorCallback(string error)
        {
            Debug.LogError(error);
        }
    }
