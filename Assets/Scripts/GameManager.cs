using Assets.Scripts.Stats;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        // Управление игровыми экранами

        #region ѕеременные

        #region ƒанные дл¤ сброса при перезапуске

        [SerializeField] private GameObject _spellGenerator;                        // генератор заклинаний
        [SerializeField] private GameObject _player;                                // игрок
        [SerializeField] private GameObject[] _checkpoints;                         // массив контрольных точек
        [SerializeField] private GameObject[] _enemies;                             // массив врагов

        #endregion

        #region Ёкраны

        private GameObject _screen;                                     // текущий экран
        private GameObject _level;                                      // текущий уровень
        [SerializeField] private GameObject _mainMenuScreen;            // начальный экран
        [SerializeField] private GameObject _levelSelectScreen;         // экран настроек
        [SerializeField] private GameObject _gameLevelScreen;           // экран выхода из игры
        [SerializeField] private GameObject _defeatedMenuScreen;        // экран выхода из игры

        #endregion

        #endregion

        #region ћетоды управлени¤ экранами

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

        /// <summary>
        /// Метод выхода из игрового уровня
        /// </summary>
        public void CloseWindow()
        {
            if (!_spellGenerator.GetComponent<SpellGenerator>().IsAnimating) { DefeatedMenu(); }
        }

        #endregion

        /// <summary>
        /// Метод сброса настроек
        /// </summary>
        private void ResetData()
        {
            // сбрасываем здоровье игрока
            _player.GetComponent<Health>().ResetHealth();
            // сбрасываем защиту игрока
            _player.GetComponent<Defense>().ResetDefense();
            // сбрасываем позицию игрока
            _player.GetComponent<PlayerController>().PlayerPositionReset();

            // сбрасываем контрольные точки
            foreach (var c in _checkpoints)
            {
                c.SetActive(false);
            }
            _checkpoints[0].SetActive(true);

            // сбрасываем врагов
            foreach (var e in _enemies)
            {
                e.SetActive(false);
                e.GetComponent<Health>().ResetHealth();
            }

            // уничтожаем сундуки
            var chests = GameObject.FindGameObjectsWithTag("Chest");
            foreach (var c in chests)
            {
                Destroy(c);
            }
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
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
