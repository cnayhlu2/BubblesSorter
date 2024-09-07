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
        public bool HasView;
        public bool isMovingPrev;

        private Color GetColor => bubbleType switch
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
            view.OnBoom += BoomBubble;
            view.transform.position = position;
            view.SetColor(GetColor);
            view.gameObject.SetActive(true);
            HasView = true;
            isMovingPrev = true;
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
            HasView = false;
            gameController.OnBubbleDestroy();
            Remove();
        }

        public void Remove()
        {
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
            view.OnBoom -= BoomBubble;
            view = null;
        }

        private void OnBubbleClick()
        {
            if(_joint2D==null)
                return;
            _joint2D.enabled = false;
            _joint2D = null;
        }
    }
}