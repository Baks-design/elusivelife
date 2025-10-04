using System;
using EngineRoom.Examples.Interfaces;
using UnityEngine;
using VContainer.Unity;

namespace EngineRoom.Examples.Services
{
    public class TimeService : ITimeService, ITickable
    {
        float elapsedTime;
        
        public int SecondsPassed { get; private set; }

        public event Action SecondPassed;

        public void Tick()
        {
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime < 1f)
                return;
            
            elapsedTime = 0f;
            SecondsPassed++;
            SecondPassed?.Invoke();
        }
    }
}