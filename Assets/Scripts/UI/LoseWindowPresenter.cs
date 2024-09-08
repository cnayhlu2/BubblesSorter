using UnityEngine;

namespace TestGame
{
    public class LoseWindowPresenter : MonoBehaviour
    {
        [SerializeField] private LoseWindowView view;
        [SerializeField] private SceneController sceneController;

        private void Awake()
        {
            view.ExitButton.onClick.AddListener(ExitClick);
            view.RestartButton.onClick.AddListener(RestartClick);
            view.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            view.ExitButton.onClick.RemoveAllListeners();
            view.RestartButton.onClick.RemoveAllListeners();
        }

        private void RestartClick()
        {
            Debug.Log("RestartClick");
            sceneController.StartGame();
            view.gameObject.SetActive(false);
        }

        private void ExitClick()
        {
            Debug.Log("ExitClick");
            sceneController.MainMenu();
            view.gameObject.SetActive(false);
        }

        public void Show(int score)
        {
            view.SetScore($"{score}");
            view.gameObject.SetActive(true);
        }
    }
}