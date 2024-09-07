using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TestGame
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counter;
        [SerializeField] private TextMeshProUGUI addValueLabel;

        private const float AddDuration = 1f;
        private Vector3 startPosition;
        private Tweener tweener;

        private void Awake()
        {
            startPosition = addValueLabel.transform.position;
        }

        public void SetValue(string value) => counter.text = value;

        public void ShowAddAnimation(string value)
        {
            if (tweener != null)
                tweener.Kill(true);

            addValueLabel.text = value;
            addValueLabel.transform.position = startPosition;

            tweener = addValueLabel.transform.DOMoveY(startPosition.y + 100, AddDuration).OnComplete(() => addValueLabel.gameObject.SetActive(false));
            addValueLabel.gameObject.SetActive(true);
        }
    }
}