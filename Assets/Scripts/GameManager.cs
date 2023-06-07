using Assets.Scripts.Stats;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        // ”правление игровыми экранами

        #region ѕеременные

        #region ƒанные дл¤ сброса при перезапуске

        [SerializeField] private GameObject _spellGenerator;                        // √енератор заклинаний
        [SerializeField] private GameObject _player;                                // »грок
        [SerializeField] private GameObject[] _checkpoints;                         // ћассив контрольных точек
        [SerializeField] private GameObject[] _enemies;                             // ћассив врагов

        #endregion

        #region Ёкраны

        private GameObject _screen;                                     // текущий экран
        private GameObject _level;                                      // текущий уровень
        [SerializeField] private GameObject _mainMenuScreen;            // Ќачальный экран
        [SerializeField] private GameObject _levelSelectScreen;         // Ёкран настроек
        [SerializeField] private GameObject _gameLevelScreen;           // Ёкран выхода из игры
        [SerializeField] private GameObject _defeatedMenuScreen;        // Ёкран выхода из игры

        #endregion

        #endregion

        #region ћетоды управлени¤ экранами

        /// <summary>
        /// ћетод вызова экрана главного меню
        /// </summary>
        public void MainMenu()
        {
            Time.timeScale = 0;
            _screen.SetActive(false);
            _mainMenuScreen.SetActive(true);
            _screen = _mainMenuScreen;
        }

        /// <summary>
        /// ћетод вызова экрана выбора уровн¤
        /// </summary>
        public void LevelSelect()
        {
            Time.timeScale = 0;
            _screen.SetActive(false);
            _levelSelectScreen.SetActive(true);
            _screen = _levelSelectScreen;
        }

        /// <summary>
        /// ћетод вызова экрана игрового уровн¤
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
        /// ћетод вызова перезапуска игрового уровн¤
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
        /// ћетод вызова экрана поражени¤
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
        /// ћетод выхода из игрового уровн¤
        /// </summary>
        public void CloseWindow()
        {
            if (!_spellGenerator.GetComponent<SpellGenerator>().IsAnimating) { DefeatedMenu(); }
        }

        #endregion

        /// <summary>
        /// ћетод сброса настроек
        /// </summary>
        private void ResetData()
        {
            // —брасываем здоровье игрока
            _player.GetComponent<Health>().ResetHealth();
            // —брасываем защиту игрока
            _player.GetComponent<Defense>().ResetDefense();
            // —брасываем позицию игрока
            _player.GetComponent<PlayerController>().PlayerPositionReset();

            // —брасываем контрольные точки
            foreach (var c in _checkpoints)
            {
                c.SetActive(false);
            }
            _checkpoints[0].SetActive(true);

            // —брасываем врагов
            foreach (var e in _enemies)
            {
                e.SetActive(false);
                e.GetComponent<Health>().ResetHealth();
            }

            // ”ничтожаем сундуки
            var chests = GameObject.FindGameObjectsWithTag("Chest");
            foreach (var c in chests)
            {
                Destroy(c);
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
