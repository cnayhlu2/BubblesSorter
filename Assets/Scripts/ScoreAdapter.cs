using System;
using UnityEngine;

namespace TestGame
{
    public class ScoreAdapter : MonoBehaviour
    {
        [SerializeField] private ScoreView view;
        [SerializeReference] private IGameController gameController;

        private int score;

        private void Awake()
        {
            gameController.GameScoreUpdated += OnGameScoreUpdated;
            gameController.GameStart += OnGameStart;
        }

        private void OnDestroy()
        {
            gameController.GameScoreUpdated -= OnGameScoreUpdated;
            gameController.GameStart -= OnGameStart;
        }

        private void OnGameStart(int value)
        {
            score = value;
            view.SetValue($"{value}");
        }

        private void OnGameScoreUpdated(int newScore)
        {
            int addScore = newScore - score;
            view.SetValue($"{newScore}");
            view.ShowAddAnimation($"<color=green>+{addScore}</color>");
        }
    }
}