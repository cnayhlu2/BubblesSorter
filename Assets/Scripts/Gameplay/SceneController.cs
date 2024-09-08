using System;
using UnityEngine;

namespace TestGame
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private MainMenuController mainMenuController;

        private void Start()
        {
            mainMenuController.Show();
        }

        public void StartGame()
        {
            mainMenuController.Hide();
            gameController.StartNewGame();
        }

        public void MainMenu()
        {
            gameController.Hide();
            mainMenuController.Show();
        }
    }
}