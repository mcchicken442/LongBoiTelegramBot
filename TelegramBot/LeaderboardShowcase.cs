using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using ResourceHandler.Resources.Enums;


public class LeaderboardShowcase
    {
        private int _defaultPageNumber = 1, _defaultEntriesToTake = 100;
        private Entry[] leaderboardEntries = new Entry[1];
        
        public void Load()
        {
            var timePeriod = 
                //Dan.Enums.TimePeriodType.Today :
                //Dan.Enums.TimePeriodType.ThisWeek :
                //Dan.Enums.TimePeriodType.ThisMonth :
                //Dan.Enums.TimePeriodType.ThisYear :
                TimePeriodType.AllTime;

            var pageNumber = _defaultPageNumber;
            
            var take = _defaultEntriesToTake;;
            
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
        }
        
        private void Callback(bool success)
        {
            if (success)
                Load();
        }
        
        private void ErrorCallback(string error)
        {
            //Debug.LogError(error);
        }
    }
