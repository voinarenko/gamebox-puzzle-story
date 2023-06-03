using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Переменные

        private GameObject _screen;                                     // текущий экран
        [SerializeField] private GameObject _mainMenuScreen;            // Начальный экран
        [SerializeField] private GameObject _levelSelectScreen;         // Экран настроек
        [SerializeField] private GameObject _gameLevelScreen;           // Экран выхода из игры
        [SerializeField] private GameObject _defeatedMenuScreen;        // Экран выхода из игры

        #endregion

        #region Методы управления экранами

        /// <summary>
        /// Метод вызова экрана главного меню
        /// </summary>
        public void MainMenu()
        {
            Time.timeScale = 0;
            _screen.SetActive(false);
            _mainMenuScreen.SetActive(true);
            _screen = _mainMenuScreen;
        }

        /// <summary>
        /// Метод вызова экрана выбора уровня
        /// </summary>
        public void LevelSelect()
        {
            Time.timeScale = 0;
            _screen.SetActive(false);
            _levelSelectScreen.SetActive(true);
            _screen = _levelSelectScreen;
        }

        /// <summary>
        /// Метод вызова экрана игрового уровня
        /// </summary>
        public void GameLevel()
        {
            Time.timeScale = 1;
            _screen.SetActive(false);
            _gameLevelScreen.SetActive(true);
            _screen = _gameLevelScreen;
            PlayerController.StartGame();
        }

        /// <summary>
        /// Метод вызова перезапуска игрового уровня
        /// </summary>
        public void RestartLevel()
        {
            Time.timeScale = 1;
            _screen.SetActive(false);
            ResetData();
            _gameLevelScreen.SetActive(true);
            _screen = _gameLevelScreen;
        }

        /// <summary>
        /// Метод вызова экрана поражения
        /// </summary>
        public void DefeatedMenu()
        {
            SpellGenerator.ClearTheField();
            Time.timeScale = 0;
            _screen.SetActive(false);
            _defeatedMenuScreen.SetActive(true);
            _screen = _defeatedMenuScreen;
        }

        #endregion

        /// <summary>
        /// Метод сброса настроек
        /// </summary>
        private void ResetData()
        {
            //
        }

        private void Start()
        {
            _screen = _mainMenuScreen;
            _mainMenuScreen.SetActive(true);
            _levelSelectScreen.SetActive(false);
            _gameLevelScreen.SetActive(false);
            _defeatedMenuScreen.SetActive(false);
        }
    }


}
