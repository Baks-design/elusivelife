using System;
using EngineRoom.Examples.Interfaces;
using TMPro;
using UnityEngine;
using VContainer;

namespace EngineRoom.Examples.Views
{
    public class UiView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI timeText;
        ITimeService timeService;
        IScoreService scoreService;
        
        [Inject]
        public void Construct(ITimeService timeService, IScoreService scoreService)
        {
           this. timeService = timeService;
            this.scoreService = scoreService;
        }

        void Awake()
        {
            timeService.SecondPassed += OnSecondPassed;
            scoreService.ScoreChanged += OnScoreChanged;
        }

        void OnScoreChanged(int newScore) => scoreText.text = $"Score: {newScore}";

        void OnSecondPassed()
        {
            var timePassed = TimeSpan.FromSeconds(timeService.SecondsPassed);
            timeText.text = $"Time: {timePassed:mm\\:ss}";
        }

        void OnDestroy()
        {
            timeService.SecondPassed -= OnSecondPassed;
            scoreService.ScoreChanged -= OnScoreChanged;
        }
    }
}