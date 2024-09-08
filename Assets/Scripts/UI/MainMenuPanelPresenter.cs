using UnityEngine;

namespace TestGame
{
    public class MainMenuPanelPresenter : MonoBehaviour
    {
        [SerializeField] private MainMenuPanelView view;
        [SerializeField] private MainMenuController menuController;
        [SerializeField] private SceneController sceneController;
        
        private void Awake()
        {
            view.StartGameButton.onClick.AddListener(GameStart);
            menuController.OnShow += Show;
            menuController.OnHide += Hide;
            Hide();
        }

        private void OnDestroy()
        {
            menuController.OnShow -= Show;
            menuController.OnHide -= Hide;
            view.StartGameButton.onClick.RemoveAllListeners();
        }

        private void Show()
        {
            view.gameObject.SetActive(true);
        }

        private void Hide()
        {
            view.gameObject.SetActive(false);
        }

        private void GameStart()
        {
            sceneController.StartGame();
        }
    }
}