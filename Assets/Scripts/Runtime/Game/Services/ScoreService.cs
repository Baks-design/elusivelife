using System;
using EngineRoom.Examples.Interfaces;

namespace EngineRoom.Examples.Services
{
    public class ScoreService : IScoreService
    {
         readonly IGameSettings gameSettings;

        public event Action<int> ScoreChanged;
        
        public int CurrentScore { get; private set; }
        
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