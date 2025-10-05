using System;

namespace ElusiveLife.Game.Score
{
    public interface IScoreService
    {
        int CurrentScore { get; }

        event Action<int> ScoreChanged;

        void AddScore();
        void SubtractScore();
    }
}