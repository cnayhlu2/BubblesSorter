using System;
using UnityEngine;

namespace TestGame
{
    public class BubblePresenter : IDisposable
    {
        public const float threshold = 0.01f;

        private BubbleView view;
        private Joint2D _joint2D;
        private BubbleType bubbleType;
        private BubbleViewCreator creator;
        private IGameController gameController;
        private bool isMovingPrev;

        public bool HasView => view != null && view.Body.simulated;

        public static Color GetColor(BubbleType bubbleType) => bubbleType switch
        {
            BubbleType.Blue => Color.blue,
            BubbleType.Green => Color.green,
            _ => Color.red,
        };

        public Vector2 Position => view.transform.position;
        public BubbleType BubbleType => bubbleType;

        public BubblePresenter(BubbleViewCreator creator, Vector2 position, BubbleType type, IGameController gameController)
        {
            this.creator = creator;
            this.gameController = gameController;
            bubbleType = type;
            view = creator.CreateBubble();
            view.OnClick += OnBubbleClick;
            view.ParticleStopEventMediator.OnExplosionEffectStop += OnExplosionEffectStop;
            view.transform.position = position;
            view.SetColor(GetColor(type));
            view.gameObject.SetActive(true);
            isMovingPrev = true;
        }

        private void OnExplosionEffectStop()
        {
            Remove();
        }

        public bool IsIfMoving()
        {
            bool isMovingNow = view.Body.velocity.magnitude >= threshold;

            if (isMovingPrev)
            {
                isMovingPrev = isMovingNow;
                return true;
            }

            isMovingPrev = isMovingNow;
            return isMovingPrev;
        }

        public void BoomBubble()
        {
            view.ShowBoom();
            gameController.OnBubbleDestroy();
        }

        public void Remove()
        {
            gameController.OnBubbleDestroy();
            creator.ReleaseBubble(view);
            Dispose();
        }

        public void ConnectToJoint(DistanceJoint2D joint2D)
        {
            _joint2D = joint2D;
            joint2D.connectedBody = view.Body;
            _joint2D.enabled = true;
            view.Body.simulated = true;
        }

        public void Dispose()
        {
            view.OnClick -= OnBubbleClick;
            view.ParticleStopEventMediator.OnExplosionEffectStop -= OnExplosionEffectStop;
        }

        private void OnBubbleClick()
        {
            if (_joint2D == null)
                return;
            _joint2D.enabled = false;
            _joint2D = null;
        }
    }
}