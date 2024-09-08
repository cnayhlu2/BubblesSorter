using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static TestGame.Utilities;

namespace TestGame
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private BubbleViewCreator creator;
        [SerializeField] private GameObject destorer;

        private const float yPosition = 5f;
        private const float creationWidth = 3f;

        public event Action OnShow;
        public event Action OnHide;

        private CancellationTokenSource cts;

        public void Hide()
        {
            if (cts != null)
                cts.Cancel();
            OnHide?.Invoke();
            destorer.gameObject.SetActive(false);
        }

        public void Show()
        {
            StartBubbleCreation();
            OnShow?.Invoke();
            destorer.gameObject.SetActive(true);
        }


        private void StartBubbleCreation()
        {
            cts = new CancellationTokenSource();
            BubbleCreation(cts.Token);
        }

        private async UniTask BubbleCreation(CancellationToken token)
        {
            List<BubbleView> bubbleViews = new();
            try
            {
                while (true)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(.5f, 1.5f)), cancellationToken: token);
                    var bubble = creator.CreateBubble();
                    bubbleViews.Add(bubble);
                    bubble.transform.position = new Vector2(UnityEngine.Random.Range(-creationWidth, creationWidth), yPosition);
                    bubble.SetColor(BubblePresenter.GetColor(RandomEnumValue<BubbleType>()));
                    bubble.gameObject.SetActive(true);
                    bubble.ParticleStopEventMediator.OnExplosionEffectStop += () =>
                    {
                        bubbleViews.Remove(bubble);
                        creator.ReleaseBubble(bubble);
                    };
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Infinite loop canceled.");

                foreach (var bubbleView in bubbleViews)
                {
                    creator.ReleaseBubble(bubbleView);
                }
                
            }
        }
    }
}