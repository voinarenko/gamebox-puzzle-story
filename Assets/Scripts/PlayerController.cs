using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        // ”правление игровым процессом в зоне Side

        #region ѕеременные

        [SerializeField] private SpellGenerator _spellGenerator;    // генератор заклинаний

        [SerializeField] private GameObject _eventSystem;           // обработка событий
        [SerializeField] private GameObject[] _enemies;             // массив врагов
        [SerializeField] private GameObject[] _checkpoints;         // массив контрольных точек
        [SerializeField] private GameObject[] _chests;              // массив сундуков

        public int CurrentEpisode = 1;                              // текущий эпизод
        private static int _episode;                                // переменна¤ дл¤ переключател¤ эпизодов

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
        /// ћетод запуска игрового сценари¤
        /// </summary>
        public static void StartGame()
        {
            _episode = 1;
        }

        /// <summary>
        /// ћетод, мен¤ющий текущий эпизод
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
        /// ћетод выбора эпизода
        /// </summary>
        /// <param name="episode">номер эпизода</param>
        private void EpisodeSelector(int episode)
        {
            switch (episode)
            {
                case 1:
                    PlayerStoryMovement(0, 1, 0);       // Ёпизод 1
                    break;
                case 2:
                    PlayerStoryMovement(1, 2, 0);       // Ёпизод 2
                    break;
                case 3:
                    PlayerStoryMovement(2, 3, 1);       // Ёпизод 3
                    break;
                case 4:
                    PlayerStoryMovement(3, 0, 2);       // Ёпизод 4
                    break;
            }
        }

        /// <summary>
        /// ћетод, вызывающий процессы между сражени¤ми
        /// </summary>
        /// <param name="enemy">позици¤ врага</param>
        /// <param name="checkpoint">позици¤ контрольной точки</param>
        /// <param name="chest">позици¤ сундука</param>
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
        /// ћетод, вызывающий сундук
        /// </summary>
        /// <param name="value">позици¤ сундука</param>
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
        /// ћетод сброса позиции игрока
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
