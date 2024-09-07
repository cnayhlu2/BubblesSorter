using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace TestGame
{
    public class ScoreAdapter : SerializedMonoBehaviour
    {
        [SerializeField] private ScoreView view;
        [OdinSerialize] private IGameController gameController;

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

        [Button]
        private void OnGameStart(int value)
        {
            score = value;
            view.SetValue($"{value}");
        }

        [Button]
        private void OnGameScoreUpdated(int newScore)
        {
            int addScore = newScore - score;
            score = newScore;
            view.SetValue($"{newScore}");
            if (addScore > 0)
                view.ShowAddAnimation($"<color=green>+{addScore}</color>");
        }
    }
}