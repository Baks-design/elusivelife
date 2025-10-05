using System;
using VContainer.Unity;

namespace ElusiveLife.Application.Time
{
    public class TimeService : ITimeService, ITickable
    {
        float elapsedTime;

        public int SecondsPassed { get; private set; }

        public event Action SecondPassed;

        public void Tick()
        {
            elapsedTime += UnityEngine.Time.deltaTime;
            if (elapsedTime < 1f)
                return;

            elapsedTime = 0f;
            SecondsPassed++;
            SecondPassed?.Invoke();
        }
    }
}