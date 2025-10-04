using System;

namespace EngineRoom.Examples.Interfaces
{
    public interface IScoreService
    {
        int CurrentScore { get; }

        event Action<int> ScoreChanged;
        
        void AddScore();
        void SubtractScore();
    }
}