using System;
using ElusiveLife.Game.Settings;

namespace ElusiveLife.Game.Score
{
    public class ScoreService : IScoreService
    {
        readonly IGameSettings gameSettings;

        public int CurrentScore { get; private set; }

        public event Action<int> ScoreChanged;

        public ScoreService(IGameSettings gameSettings)
        => this.gameSettings = gameSettings;

        public void AddScore()
        {
            CurrentScore += gameSettings.PointsPerHit;
            ScoreChanged?.Invoke(CurrentScore);
        }

        public void SubtractScore()
        {
            CurrentScore += gameSettings.PointsPerMiss;
            ScoreChanged?.Invoke(CurrentScore);
        }
    }
}