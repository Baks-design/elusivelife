using System;
using VContainer.Unity;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Time
{
    public class TimeService : ITimeService, ITickable
    {
        private float _elapsedTime;

        public int SecondsPassed { get; private set; }

        public event Action SecondPassed;

        public void Tick()
        {
            _elapsedTime += UnityEngine.Time.deltaTime;
            if (_elapsedTime < 1f) return;

            _elapsedTime = 0f;
            SecondsPassed++;
            SecondPassed?.Invoke();
        }
    }
}