using System;
using UnityEngine;

namespace TestGame
{
    public class BubbleView : MonoBehaviour
    {
        public Rigidbody2D Body;
        public event Action OnClick;
        public event Action OnBoom;
        [SerializeField] private SpriteRenderer _image;

        private void OnMouseDown()
        {
            OnClick?.Invoke();
        }

        public void ShowBoom()
        {
            OnBoom?.Invoke();   
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}