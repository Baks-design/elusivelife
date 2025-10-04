using System;

namespace EngineRoom.Examples.Interfaces
{
    public interface ITimeService
    {
        int SecondsPassed { get; }

        event Action SecondPassed;
    }
}