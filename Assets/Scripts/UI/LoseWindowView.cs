using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestGame
{
    public class LoseWindowView : MonoBehaviour
    {
        public Button RestartButton;
        public Button ExitButton;

        [SerializeField] private TextMeshProUGUI score;
        public void SetScore(string value) => score.text = value;
    }
}