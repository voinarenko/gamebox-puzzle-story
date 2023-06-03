using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region ����������

        private GameObject _screen;                                     // ������� �����
        [SerializeField] private GameObject _mainMenuScreen;            // ��������� �����
        [SerializeField] private GameObject _levelSelectScreen;         // ����� ��������
        [SerializeField] private GameObject _gameLevelScreen;           // ����� ������ �� ����
        [SerializeField] private GameObject _defeatedMenuScreen;        // ����� ������ �� ����

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
            ResetData();
            _gameLevelScreen.SetActive(true);
            _screen = _gameLevelScreen;
        }

        /// <summary>
        /// ����� ������ ������ ���������
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
        /// ����� ������ ��������
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
