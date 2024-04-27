﻿
    [System.Serializable]
    public struct Entry
    {
        public string Username;
        public int Score;
        public ulong Date;
        public string Extra;
        public int Rank;
        internal string UserGuid;
        [field: System.NonSerialized] internal string NewUsername { get; set; }
        

        /// <summary>
        /// Returns the rank of the entry with its suffix.
        /// </summary>
        /// <returns>Rank + suffix (e.g. 1st, 2nd, 3rd, 4th, 5th, etc.).</returns>
        public string RankSuffix()
        {
            var rank = Rank;
            var lastDigit = rank % 10;
            var lastTwoDigits = rank % 100;

            var suffix = lastDigit == 1 && lastTwoDigits != 11 ? "st" :
                lastDigit == 2 && lastTwoDigits != 12 ? "nd" :
                lastDigit == 3 && lastTwoDigits != 13 ? "rd" : "th";

            return $"{rank}{suffix}";
        }
    }