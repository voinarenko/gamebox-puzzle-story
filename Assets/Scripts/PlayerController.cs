using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        // Управление игровым процессом в зоне Side

        #region ѕеременные

        [SerializeField] private SpellGenerator _spellGenerator;    // генератор заклинаний

        [SerializeField] private GameObject _eventSystem;           // обработка событий
        [SerializeField] private GameObject[] _enemies;             // массив врагов
        [SerializeField] private GameObject[] _checkpoints;         // массив контрольных точек
        [SerializeField] private GameObject[] _chests;              // массив сундуков

        public int CurrentEpisode = 1;                              // текущий эпизод
        private static int _episode;                                // переменная для переключателя эпизодов

        private Vector3 _startPosition;                             // начальное положение игрока

        #endregion

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _startPosition = gameObject.transform.position;
        }

        /// <summary>
        /// Метод запуска игрового сценария
        /// </summary>
        public static void StartGame()
        {
            _episode = 1;
        }

        /// <summary>
        /// метод, меняющий текущий эпизод
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
        /// Метод выбора эпизода
        /// </summary>
        /// <param name="episode">номер эпизода</param>
        private void EpisodeSelector(int episode)
        {
            switch (episode)
            {
                case 1:
                    PlayerStoryMovement(0, 1, 0);       // эпизод 1
                    break;
                case 2:
                    PlayerStoryMovement(1, 2, 0);       // эпизод 2
                    break;
                case 3:
                    PlayerStoryMovement(2, 3, 1);       // эпизод 3
                    break;
                case 4:
                    PlayerStoryMovement(3, 0, 2);       // эпизод 4
                    break;
            }
        }

        /// <summary>
        /// Метод, вызывающий процессы между сражениями
        /// </summary>
        /// <param name="enemy">позиция врага</param>
        /// <param name="checkpoint">позиция контрольной точки</param>
        /// <param name="chest">позиция сундука</param>
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
        /// Метод, вызывающий сундук
        /// </summary>
        /// <param name="value">позиция сундука</param>
        private void GenerateChest(int value)
        {
            _spellGenerator.IsAnimating = true;
            var position = new Vector3(0, 0, 80);
            var chest = Instantiate(_chests[value], position, Quaternion.identity);
            chest.transform.DOScale(new Vector3(2, 2, 2), 1f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                _spellGenerator.IsAnimating = false;
            });
        }
        
        /// <summary>
        /// Метод сброса позиции игрока
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
