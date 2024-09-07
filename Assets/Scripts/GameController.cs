using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static TestGame.Utilities;

namespace TestGame
{
    public class GameController : MonoBehaviour, IGameController
    {
        private int gameScore;

        public event Action<int> GameScoreUpdated;
        public event Action<int> GameStart;

        private int GameScore
        {
            set
            {
                gameScore = value;
                GameScoreUpdated?.Invoke(gameScore);
            }
            get => gameScore;
        }

        private const int CountToLose = 9;
        private const float HeightPositionToLose = -1.8f;

        [SerializeField] private BubbleViewCreator creator;
        [SerializeField] private List<Vector2> _startingPositions = new();

        [SerializeField] private DistanceJoint2D joint2D;

        private readonly List<BubblePresenter> bubblePresenters = new();

        private bool isWaitingAction = true;

        private float currentTime;
        private readonly float checkDistanceTime = .25f;

        private bool gameCompleted;

        private readonly Vector2[,] matrixPosition = new Vector2[,]
        {
            {new(-1.1f, -2.44f), new(0.1f, -2.44f), new(1.38f, -2.44f)},
            {new(-1.1f, -3.2f), new(0.1f, -3.2f), new(1.38f, -3.2f)},
            {new(-1.1f, -4.1f), new(0.1f, -4.1f), new(1.38f, -4.1f)}
        };


        private readonly int[][] checkLines =
        {
            new[] {0, 3, 6}, new[] {1, 4, 7}, new[] {2, 5, 8},
            new[] {0, 1, 2}, new[] {3, 4, 5}, new[] {6, 7, 8},
            new[] {0, 4, 8}, new[] {6, 4, 2},
        };

        public void Start()
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            foreach (var bubblePresenter in bubblePresenters)
            {
                bubblePresenter.Remove();
            }

            isWaitingAction = false;
            gameCompleted = false;
            gameScore = 0;
            GameStart?.Invoke(gameScore);
        }


        private void Update()
        {
            if (gameCompleted)
                return;

            if (isWaitingAction)
                return;

            if (joint2D.enabled)
                return;

            currentTime += Time.deltaTime;

            if (currentTime < checkDistanceTime)
                return;

            currentTime = 0;

            if (!AllBubblesIsNotMoving())
                return;

            if (CheckLoseCondition(LoseConditions.TopLine))
            {
                Lose(LoseConditions.TopLine);
                return;
            }

            if (TryRemoveBubbles())
            {
                return;
            }

            if (CheckLoseCondition(LoseConditions.ManyBalls))
            {
                Lose(LoseConditions.ManyBalls);
                return;
            }

            InstantiateBubble();
        }

        private bool CheckLoseCondition(LoseConditions condition)
        {
            if (condition == LoseConditions.TopLine)
            {
                foreach (var bubblePresenter in bubblePresenters)
                {
                    if (bubblePresenter.Position.y > HeightPositionToLose)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (bubblePresenters.Count > CountToLose)
                {
                    return true;
                }
            }

            return false;
        }

        private void Lose(LoseConditions conditions)
        {
            Debug.Log($"Lose conditions: {conditions}");
            gameCompleted = true;
        }


        private bool TryRemoveBubbles()
        {
            Dictionary<int, BubblePresenter> helpHDictionary = new();

            for (var i = 0; i < matrixPosition.GetLength(1); i++)
            {
                for (var j = matrixPosition.GetLength(0) - 1; j >= 0; j--)
                {
                    foreach (var bubblePresenter in bubblePresenters)
                    {
                        var magnitudeSqr = (bubblePresenter.Position - matrixPosition[j, i]).sqrMagnitude;
                        if (magnitudeSqr < .3f)
                        {
                            helpHDictionary.Add(ToIndex(j, i), bubblePresenter);
                            break;
                        }
                    }

                    var index = ToIndex(j, i);
                    if (helpHDictionary.ContainsKey(index))
                    {
                        continue;
                    }
                    break;
                }
            }

            List<int> findLines = new List<int>();

            for (var index = 0; index < checkLines.Length; index++)
            {
                var checkLine = checkLines[index];
                var completeLine = true;

                foreach (var checkPosition in checkLine)
                {
                    if (!helpHDictionary.ContainsKey(checkPosition))
                    {
                        completeLine = false;
                        break;
                    }
                }

                if (completeLine)
                {
                    if (helpHDictionary[checkLine[0]].BubbleType == helpHDictionary[checkLine[1]].BubbleType &&
                        helpHDictionary[checkLine[0]].BubbleType == helpHDictionary[checkLine[2]].BubbleType)
                    {
                        findLines.Add(index);
                    }
                }
            }

            int addScore = 0;
            foreach (var lineIndex in findLines)
            {
                foreach (var index in checkLines[lineIndex])
                {
                    var bubble = helpHDictionary[index];
                    if (!bubble.HasView) continue;
                    addScore += GetScore(bubble.BubbleType);
                    bubble.BoomBubble();
                }
            }

            GameScore += addScore;

            int GetScore(BubbleType bubbleType) => bubbleType switch
            {
                BubbleType.Blue => 3,
                BubbleType.Green => 2,
                _ => 1,
            };

            int ToIndex(int height, int width)
            {
                return (height * 3) + width;
            }

            return false;
        }

        private bool AllBubblesIsNotMoving()
        {
            Debug.Log($"bubblePresenters count {bubblePresenters.Count}");
            foreach (var bubblePresenter in bubblePresenters)
            {
                if (bubblePresenter.IsIfMoving())
                {
                    return false;
                }
            }

            return true;
        }

        private void InstantiateBubble()
        {
            var bubbleType = RandomEnumValue<BubbleType>();

            while (bubbleType == BubbleType.None)
            {
                bubbleType = RandomEnumValue<BubbleType>();
            }

            var bubble = new BubblePresenter(creator, _startingPositions.GetRandomValue(),bubbleType , this);
            bubble.ConnectToJoint(joint2D);
            bubblePresenters.Add(bubble);
        }

        public void OnBubbleDestroy()
        {
            for (int i = bubblePresenters.Count - 1; i >= 0; i--)
            {
                if (!bubblePresenters[i].HasView)
                    bubblePresenters.RemoveAt(i);
            }
        }
    }
}