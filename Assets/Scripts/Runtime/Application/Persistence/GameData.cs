using System;

namespace ElusiveLife.Runtime.Application.Persistence
{
    [Serializable]
    public class GameData
    {
        public int HighScore;
        public string PlayerName;
        public DateTime LastSaveTime;
        public int TotalPlayTime; 
        public int GamesPlayed;
        public string Version = "0.0.1";

        public GameData()
        {
            PlayerName = "Player";
            LastSaveTime = DateTime.Now;
        }
    }
}