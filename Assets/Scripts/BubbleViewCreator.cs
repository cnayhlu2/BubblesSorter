using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TestGame
{
    public class BubbleViewCreator : MonoBehaviour
    {
        [SerializeField] private BubbleView _bubblePrefab;
        [SerializeField] private Transform _bubbleRoot;

        private IObjectPool<BubbleView> _views;

        private void Awake()
        {
            _views = new LinkedPool<BubbleView>(Create, Get, OnRelease);
        }

        public BubbleView CreateBubble()
        {
            return _views.Get();
        }

        public void ReleaseBubble(BubbleView view)
        {
            _views.Release(view);
        }

        private void OnRelease(BubbleView obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void Get(BubbleView view)
        {
            view.Get();
        }

        private BubbleView Create()
        {
            var view = Instantiate(_bubblePrefab, _bubbleRoot);
            view.gameObject.SetActive(false);
            return view;
        }
    }
}