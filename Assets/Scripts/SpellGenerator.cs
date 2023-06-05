using Assets.Scripts.Stats;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class SpellGenerator : MonoBehaviour
    {
        // Генератор заклинаний

        #region Объявление переменных

        public bool IsAnimating;                                                    // состояние анимации для блокировки нажатий    

        [SerializeField] private GameObject _eventSystem;                           // обработка событий

        private GameObject[] _nests = new GameObject[30];                           // Массив объектов с координатами
        private static int _nestId;                                                 // Номер позиции в массиве

        private bool _firstSelected;                                                // Наличие выделенного объекта
        private string _firstSelectedTag;                                           // Тэг выделенного объекта
        private int _firstSelectedTier;                                             // Уровень выделенного объекта

        [SerializeField] private int _enemyMove = 3;                                // ход врага
        private int _moveCounter;                                                   // счётчик ходов

        #region Массивы заклинаний

        [SerializeField] private GameObject[] _spellsTier1 = new GameObject[3];    // Массив заклинаний 1-го уровня
        [SerializeField] private GameObject[] _spellsTier2 = new GameObject[3];    // Массив заклинаний 2-го уровня
        [SerializeField] private GameObject[] _spellsTier3 = new GameObject[3];    // Массив заклинаний 3-го уровня

        #endregion

        #endregion

        #region Методы

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _nests = GameObject.FindGameObjectsWithTag("Nest");                     // Заполняем массив объектами
        }

        #region Методы выдачи значений переменных

        /// <summary>
        /// Метод получения значения наличия выделенного объекта
        /// </summary>
        /// <returns>наличие выделенного объекта</returns>
        public bool GetFirstSelected()
        {
            return _firstSelected;
        }

        /// <summary>
        /// Метод получения значения тэга выделенного объекта
        /// </summary>
        /// <returns>значение тэга</returns>
        public string GetFirstSelectedTag()
        {
            return _firstSelectedTag;
        }

        /// <summary>
        /// Метод получения значения уровня выделенного объекта
        /// </summary>
        /// <returns>значение уровня</returns>
        public int GetFirstSelectedTier()
        {
            return _firstSelectedTier;
        }

        #endregion

        #region Методы работы с полем

        /// <summary>
        /// Метод поиска свободного места на поле
        /// </summary>
        /// <returns>координаты свободного места</returns>
        private Vector3 FindSpawnPlace()
        {
            var placeIsFound = false;
            var result = new Vector3();
            while (!placeIsFound)
            {
                var rnd = new Random();
                var value = _nests.Length;
                var randomPosition = rnd.Next(value);
                if (NestIsOccupied(_nests[randomPosition]))
                {
                    continue;
                }
                _nests[randomPosition].GetComponent<OccupationStatusScript>().IsOccupied = true;
                _nestId = randomPosition;
                result = _nests[randomPosition].transform.position;
                placeIsFound = true;
            }
            return result;
        }
        
        /// <summary>
        /// Метод, снимающий метку занятости после уничтожения объекта
        /// </summary>
        /// <param name="target">уничтоженный объект</param>
        private void FreeNest(GameObject target)
        {
            _nests[target.GetComponent<SpellData>().NestId].GetComponent<OccupationStatusScript>().IsOccupied = false;
        }

        /// <summary>
        /// Метод проверки заполненности поля
        /// </summary>
        /// <returns>заполненность поля</returns>
        private bool FieldIsFull()
        {
            var occupiedNests = _nests.Count(nest => nest.GetComponent<OccupationStatusScript>().IsOccupied);
            return occupiedNests == 30;
        }

        /// <summary>
        /// Метод проверки занятости места
        /// </summary>
        /// <param name="nest">проверяемый объект</param>
        /// <returns>занятость места</returns>
        private static bool NestIsOccupied(GameObject nest)
        {
            return nest.GetComponent<OccupationStatusScript>().IsOccupied;
        }

        /// <summary>
        /// Метод заполнения поля заклинаниями
        /// </summary>
        public void FillTheField()
        {
            while (!FieldIsFull())
            {
                var spell = SelectRandomSpell();
                var position = FindSpawnPlace();
                SpawnSpell(_spellsTier1[spell], position, _nestId);
            }
        }

        /// <summary>
        /// Метод, очищающий игровое поле
        /// </summary>
        public static void ClearTheField()
        {
            var spellsLeft = FindObjectsOfType<SpellData>();
            foreach ( var spell in spellsLeft) { Destroy(spell.gameObject); }

            var nestsLeft = FindObjectsOfType<OccupationStatusScript>();
            foreach (var nest in nestsLeft) { nest.IsOccupied = false; }
        }

        #endregion

        #region Методы работы с заклинаниями

        /// <summary>
        /// Метод выбора случайного заклинания в соответствии с его весом
        /// </summary>
        /// <returns>случайный индекс</returns>
        private int SelectRandomSpell()
        {
            var spellSelected = false;
            var result = 0;

            while (!spellSelected)
            {
                var rnd = new Random();
                var i = rnd.Next(100);      // случайное значение в диапазоне весов
                for (var j = 0; j < _spellsTier1.Length; j++)
                {
                    // проверяем вхождение в диапазон весов заклинания
                    if (i < _spellsTier1[j].GetComponent<SpellData>().MinProbabilityRange ||
                        i > _spellsTier1[j].GetComponent<SpellData>().MaxProbabilityRange) continue;
                    result = j;
                    spellSelected = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Метод генерации заклинания
        /// </summary>
        /// <param name="spell">вызываемый объект</param>
        /// <param name="position">координаты</param>
        /// <param name="place">позиция в массиве</param>
        private void SpawnSpell(GameObject spell, Vector3 position, int place)
        {
            var newSpell = Instantiate(spell, position, Quaternion.identity);
            newSpell.GetComponent<SpellData>().NestId = place;
        }

        /// <summary>
        /// Метод определения типа заклинания по тэгу объекта
        /// </summary>
        /// <returns>номер позиции в массиве</returns>
        private int FindSpell()
        {
            return _firstSelectedTag switch
            {
                "AttackSpell" => 0,
                "HealthSpell" => 1,
                "DefenseSpell" => 2,
                _ => 0
            };
        }

        /// <summary>
        /// Метод создания заклинания выше уровнем, чем слитые
        /// </summary>
        /// <param name="position">координаты создания</param>
        /// <param name="place">позиция в массиве</param>
        public void NextTierSpell(Vector3 position, int place)
        {
            var spell = FindSpell();
            switch (_firstSelectedTier)
            {
                case 1:
                    PerformAction(spell);
                    SpawnSpell(_spellsTier2[spell], position, place);
                    break;
                case 2:
                    PerformAction(spell);
                    SpawnSpell(_spellsTier3[spell], position, place);
                    break;
                case 3:
                    PerformAction(spell);
                    _nests[place].GetComponent<OccupationStatusScript>().IsOccupied = false;
                    break;
            }
        }

        /// <summary>
        /// Метод, выполняющий действие, в зависимости от слитого заклинания
        /// </summary>
        /// <param name="spell"></param>
        public void PerformAction(int spell)
        {
            var player = GameObject.FindWithTag("Player");      // ищем игрока
            var enemy = GameObject.FindWithTag("Enemy");        // ищем врага

            // ищем заклинание
            switch (spell)
            { 
                case 0:     // атака
                    enemy.GetComponent<Health>().TakeDamage(player.GetComponent<DamageDealer>().GetDamage() * _firstSelectedTier);
                    break;
                case 1:     // здоровье
                    player.GetComponent<Health>().Heal(player.GetComponent<Health>().GetHealAmount() * _firstSelectedTier);
                    break;
                case 2:     // защита
                    player.GetComponent<Defense>().SetDefense(player.GetComponent<Defense>().GetDefenseAmount() * _firstSelectedTier);
                    break;
            }
        }

        /// <summary>
        /// Метод проверки состояний игрока и врагов
        /// </summary>
        private void CheckStatus()
        {
            var player = GameObject.FindWithTag("Player");      // ищем игрока
            var enemy = GameObject.FindWithTag("Enemy");        // ищем врага

            // Если враг уничтожен, запускаем следующий эпизод
            if (enemy == null) { player.GetComponent<PlayerController>().SelectNextEpisode(); }
            else
            {
                // Считаем ходы до атаки врага
                if (_moveCounter == _enemyMove)
                {
                    player.GetComponent<Health>().TakeDamage(enemy.GetComponent<DamageDealer>().GetDamage());   // наносим урон игроку
                    _moveCounter = 0;

                    // Проверяем существование игрока
                    player = GameObject.FindWithTag("Player");

                    // Если игрок уничтожен, выводим экран поражения
                    if (player == null)
                    {
                        player.GetComponent<Health>().ResetHealth();
                        _eventSystem.GetComponent<GameManager>().DefeatedMenu();
                    }
                }
                else
                {
                    // Увеличиваем счётчик
                    _moveCounter++;
                }
            }
        }

        #endregion

        #region Методы обработки дейчствий с заклинаниями

        /// <summary>
        /// Метод выделения объекта
        /// </summary>
        /// <param name="target">объект</param>
        public void Select(GameObject target)
        {
            _firstSelectedTag =  target.tag;                                                    // присваиваем глобальной переменной тэг текущего объекта
            _firstSelectedTier = target.GetComponent<SpellData>().Tier;                         // уровень текущего объекта
            target.tag = "Selected";                                                            // меняем тэг текущего объекта
            _firstSelected = true;                                                              // отмечаем наличие выделенного объекта
            target.GetComponent<SpriteRenderer>().color = Color.yellow;                         // меняем цвет текущего объекта
        }

        /// <summary>
        /// Метод снятия выделения с объекта
        /// </summary>
        /// <param name="target">объект</param>
        public void Deselect(GameObject target)
        {
            target.tag = _firstSelectedTag;                                                     // возвращаем тэг
            _firstSelectedTag = null;                                                           // сбрасываем тэг выделенного объекта
            _firstSelected = false;                                                             // удаляем наличие выделенного объекта
            target.GetComponent<SpriteRenderer>().color = Color.white;                          // возвращаем обычный цвет
        }

        /// <summary>
        /// Метод слияния двух одинаковых объектов в один, уровнем выше
        /// </summary>
        /// <param name="target">объект</param>
        public void Merge(GameObject target)
        {
            IsAnimating = true;
            var selected = GameObject.FindWithTag("Selected");                         // находим выделенный объект
            Vector2 moveTo = target.transform.position;                                       // задаём координаты движения
            selected.transform.DOMove(moveTo, 0.5f).OnComplete(() =>                  // запускаем анимацию перемещения, по завершению:
            {
                NextTierSpell(moveTo, target.GetComponent<SpellData>().NestId);               // создаём новый объект уровнем выше вместо текущего
                FreeNest(selected);
                Destroy(selected);                                                              // уничтожаем первый объект
                Destroy(target);                                                                // и текущий
                _firstSelectedTag = null;                                                       // сбрасываем тэг выделенного объекта
                _firstSelected = false;                                                         // и наличие выделенного объекта
                CheckStatus();
                IsAnimating = false;
            });
        }

        #endregion

        #endregion
    }
}
