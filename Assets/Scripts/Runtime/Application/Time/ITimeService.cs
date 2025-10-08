using System;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Time
{
    public interface ITimeService
    {
        int SecondsPassed { get; }

        event Action SecondPassed;
    }
}