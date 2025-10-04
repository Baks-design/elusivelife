using System;
using System.Collections.Generic;
using System.Linq;
using EngineRoom.Examples.Interfaces;
using VContainer;
using VContainer.Unity;
using ZLinq;
using Random = UnityEngine.Random;

namespace EngineRoom.Examples.Services
{
    public class TargetsService : IStartable, IDisposable
    {
         readonly ITimeService timeService;
         readonly ITargetView[] _spawnPoints;
         readonly ITargetController[] spawnPointControllers;
         readonly IObjectResolver resolver;
         readonly IScoreService scoreService;

        public TargetsService(
            ITimeService timeService, 
            IEnumerable<ITargetView> spawnPoints,
            IObjectResolver resolver, 
            IScoreService scoreService)
        {
            this.timeService = timeService;
           this. resolver = resolver;
           this. scoreService = scoreService;

            _spawnPoints = spawnPoints!.ToArray();
            spawnPointControllers = new ITargetController[_spawnPoints.Length];
        }
        
        public void Start()
        {
            timeService.SecondPassed += OnSecondPassed;

            for (var i = 0; i < _spawnPoints.Length; i++)
            {
                var targetSpawnPoint = _spawnPoints[i];
                
                var controller = resolver.Resolve<ITargetController>();
                controller.SetView(targetSpawnPoint);
                
                controller.TargetGotHit += scoreService.AddScore;
                controller.TargetDespawned += scoreService.SubtractScore;
                
                spawnPointControllers[i] = controller;
            }
        }
        
         void OnSecondPassed()
        {
            var availableControllers = spawnPointControllers
                                        .AsValueEnumerable()
                                        .Where(c => c.CanSpawnTarget)
                                        .ToArray();
            var index = Random.Range(0, availableControllers.Length);
            
            if (availableControllers.Length > 0)
                availableControllers[index].SpawnTarget();
        }

        public void Dispose()
        {
            timeService.SecondPassed -= OnSecondPassed;

            foreach (var controller in spawnPointControllers)
            {
                controller.TargetGotHit -= scoreService.AddScore;
                controller.TargetDespawned -= scoreService.SubtractScore;
            }
        }
    }
}