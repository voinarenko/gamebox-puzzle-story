using Assets.Scripts.Stats;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        // Управление игровыми экранами

        #region Переменные

        #region Данные для сброса при перезапуске

        [SerializeField] private GameObject _player;                                // Игрок
        [SerializeField] private GameObject[] _checkpoints;                         // Массив контрольных точек
        [SerializeField] private GameObject[] _enemies;                             // Массив врагов

        #endregion

        #region Экраны

        private GameObject _screen;                                     // текущий экран
        private GameObject _level;                                      // текущий уровень
        [SerializeField] private GameObject _mainMenuScreen;            // Начальный экран
        [SerializeField] private GameObject _levelSelectScreen;         // Экран настроек
        [SerializeField] private GameObject _gameLevelScreen;           // Экран выхода из игры
        [SerializeField] private GameObject _defeatedMenuScreen;        // Экран выхода из игры

        #endregion

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
            _level = _gameLevelScreen;
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
            _level.SetActive(true);
            _screen = _level;
            PlayerController.StartGame();
        }

        /// <summary>
        /// Метод вызова экрана поражения
        /// </summary>
        public void DefeatedMenu()
        {
            ResetData();
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
            // Сбрасываем здоровье игрока
            _player.GetComponent<Health>().ResetHealth();
            // Сбрасываем защиту игрока
            _player.GetComponent<Defense>().ResetDefense();
            // Сбрасываем позицию игрока
            _player.GetComponent<PlayerController>().PlayerPositionReset();

            // Сбрасываем контрольные точки
            foreach (var c in _checkpoints)
            {
                c.SetActive(false);
            }
            _checkpoints[0].SetActive(true);

            // Сбрасываем врагов
            foreach (var e in _enemies)
            {
                e.SetActive(false);
                e.GetComponent<Health>().ResetHealth();
            }
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
