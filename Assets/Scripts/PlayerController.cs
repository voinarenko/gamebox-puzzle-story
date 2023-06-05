using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        // ���������� ������� ��������� � ���� Side

        #region ����������

        [SerializeField] private SpellGenerator _spellGenerator;    // ��������� ����������

        [SerializeField] private GameObject _eventSystem;       // ��������� �������
        [SerializeField] private GameObject[] _enemies;         // ������ ������
        [SerializeField] private GameObject[] _checkpoints;     // ������ ����������� �����
        [SerializeField] private GameObject[] _chests;          // ������ ��������

        public int CurrentEpisode = 1;                          // ������� ������
        private static int _episode;                            // ���������� ��� ������������� ��������

        private Vector3 _startPosition;                         // ��������� ��������� ������

        #endregion

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _startPosition = gameObject.transform.position;
        }

        /// <summary>
        /// ����� ������� �������� ��������
        /// </summary>
        public static void StartGame()
        {
            _episode = 1;
        }

        /// <summary>
        /// �����, �������� ������� ������
        /// </summary>
        public void SelectNextEpisode()
        {
            if (CurrentEpisode == 4)
            {
                _eventSystem.GetComponent<GameManager>().DefeatedMenu();
                PlayerPositionReset();
                CurrentEpisode = 0;
            }
            CurrentEpisode++;
            _episode = CurrentEpisode;
        }

        /// <summary>
        /// ����� ������ �������
        /// </summary>
        /// <param name="episode">����� �������</param>
        private void EpisodeSelector(int episode)
        {
            switch (episode)
            {
                case 1:
                    PlayerStoryMovement(0, 1, 0);       // ������ 1
                    break;
                case 2:
                    PlayerStoryMovement(1, 2, 0);       // ������ 2
                    break;
                case 3:
                    PlayerStoryMovement(2, 3, 1);       // ������ 3
                    break;
                case 4:
                    PlayerStoryMovement(3, 0, 2);       // ������ 4
                    break;
            }
        }

        /// <summary>
        /// �����, ���������� �������� ����� ����������
        /// </summary>
        /// <param name="enemy">������� �����</param>
        /// <param name="checkpoint">������� ����������� �����</param>
        /// <param name="chest">������� �������</param>
        private void PlayerStoryMovement(int enemy, int checkpoint, int chest)
        {
            _spellGenerator.IsAnimating = true;
            _episode = 0;
            var target = GameObject.FindWithTag("CheckPoint");
            transform.DOMove(target.transform.position, 5f).OnComplete(() =>
            {
                target.SetActive(false);
                _enemies[enemy].SetActive(true);
                _checkpoints[checkpoint].SetActive(true);
                GenerateChest(chest);
            });
        }

        /// <summary>
        /// �����, ���������� ������
        /// </summary>
        /// <param name="value">������� �������</param>
        private void GenerateChest(int value)
        {
            _spellGenerator.IsAnimating = true;
            var position = new Vector3(0, 0, 80);
            var chest = Instantiate(_chests[value], position, Quaternion.identity);
            chest.transform.DOScale(new Vector3(2, 2, 2), 1f).OnComplete(() =>
            {
                _spellGenerator.IsAnimating = false;
            });
        }
        
        /// <summary>
        /// ����� ������ ������� ������
        /// </summary>
        public void PlayerPositionReset()
        {
            transform.position = _startPosition;
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            EpisodeSelector(_episode);
        }
    }
}
