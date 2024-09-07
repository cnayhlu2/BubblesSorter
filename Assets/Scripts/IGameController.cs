using System;

namespace TestGame
{
    public interface IGameController
    {
        void OnBubbleDestroy();
        void StartNewGame();
        event Action<int> GameScoreUpdated;
        event Action<int> GameStart;

    }
}