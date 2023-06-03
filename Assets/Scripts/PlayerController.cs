using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        #region ����������

        [SerializeField] private GameObject[] _enemies;         // ������ ������
        [SerializeField] private GameObject[] _checkpoints;     // ������ ����������� �����
        [SerializeField] private GameObject[] _chests;          // ������ ��������

        private static int _episode;                            // ������� ������

        #endregion

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            
        }

        /// <summary>
        /// ����� ������� �������� ��������
        /// </summary>
        public static void StartGame()
        {
            _episode = 1;
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
                    PlayerStoryMovement(0, 1, 0);
                    break;
                case 2:
                    PlayerStoryMovement(1, 2, 0);
                    break;
                case 3:
                    PlayerStoryMovement(2, 3, 1);
                    break;
                case 4:
                    PlayerStoryMovement(3, 0, 2);
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
            var chest = Instantiate(_chests[value], Vector3.zero, Quaternion.identity);
            chest.transform.DOScale(new Vector3(2, 2, 2), 1f);
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
