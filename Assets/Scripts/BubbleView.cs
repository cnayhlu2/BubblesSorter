using System;
using UnityEngine;

namespace TestGame
{
    public class BubbleView : MonoBehaviour
    {
        public Rigidbody2D Body;
        public event Action OnClick;
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Collider2D _collider2D;
        public ParticleStopEventMediator ParticleStopEventMediator;

        public void Get()
        {
            _image.enabled = true;
            _collider2D.enabled = true;
            Body.simulated = true;
        }

        private void OnMouseDown()
        {
            OnClick?.Invoke();
        }

        public void ShowBoom()
        {
            _particleSystem.Play();
            _image.enabled = false;
            _collider2D.enabled = false;
            Body.simulated = false;
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}