using Assets.Scripts.Stats;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        // ���������� �������� ��������

        #region ����������

        #region ������ ��� ������ ��� �����������

        [SerializeField] private GameObject _player;                                // �����
        [SerializeField] private GameObject[] _checkpoints;                         // ������ ����������� �����
        [SerializeField] private GameObject[] _enemies;                             // ������ ������

        #endregion

        #region ������

        private GameObject _screen;                                     // ������� �����
        private GameObject _level;                                      // ������� �������
        [SerializeField] private GameObject _mainMenuScreen;            // ��������� �����
        [SerializeField] private GameObject _levelSelectScreen;         // ����� ��������
        [SerializeField] private GameObject _gameLevelScreen;           // ����� ������ �� ����
        [SerializeField] private GameObject _defeatedMenuScreen;        // ����� ������ �� ����

        #endregion

        #endregion

        #region ������ ���������� ��������

        /// <summary>
        /// ����� ������ ������ �������� ����
        /// </summary>
        public void MainMenu()
        {
            Time.timeScale = 0;
            _screen.SetActive(false);
            _mainMenuScreen.SetActive(true);
            _screen = _mainMenuScreen;
        }

        /// <summary>
        /// ����� ������ ������ ������ ������
        /// </summary>
        public void LevelSelect()
        {
            Time.timeScale = 0;
            _screen.SetActive(false);
            _levelSelectScreen.SetActive(true);
            _screen = _levelSelectScreen;
        }

        /// <summary>
        /// ����� ������ ������ �������� ������
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
        /// ����� ������ ����������� �������� ������
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
        /// ����� ������ ������ ���������
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
        /// ����� ������ ��������
        /// </summary>
        private void ResetData()
        {
            // ���������� �������� ������
            _player.GetComponent<Health>().ResetHealth();
            // ���������� ������ ������
            _player.GetComponent<Defense>().ResetDefense();
            // ���������� ������� ������
            _player.GetComponent<PlayerController>().PlayerPositionReset();

            // ���������� ����������� �����
            foreach (var c in _checkpoints)
            {
                c.SetActive(false);
            }
            _checkpoints[0].SetActive(true);

            // ���������� ������
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
