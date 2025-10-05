using System;

namespace ElusiveLife.Application.Time
{
    public interface ITimeService
    {
        int SecondsPassed { get; }

        event Action SecondPassed;
    }
}