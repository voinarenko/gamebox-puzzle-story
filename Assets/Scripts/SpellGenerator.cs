using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class SpellGenerator : MonoBehaviour
    {
        #region Объявление переменных

        private GameObject[] _nests = new GameObject[30];                           // Массив объектов с координатами
        private static int _nestId;                                                    // Номер позиции в массиве

        private bool _firstSelected;                                                // Наличие выделенного объекта
        private string _firstSelectedTag;                                           // Тэг выделенного объекта
        private int _firstSelectedTier;                                             // Уровень выделенного объекта


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

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
        
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
        /// Метод выбора случайного заклинания
        /// </summary>
        /// <returns>случайный индекс</returns>
        private int SelectRandomSpell()
        {
            var rnd = new Random();
            var result = rnd.Next(_spellsTier1.Length);
            return result;
        }

        /// <summary>
        /// Метод генерации заклинания
        /// </summary>
        /// <param name="spell">вызываемый объект</param>
        /// <param name="position">координаты</param>
        /// <param name="place">позиция в массиве</param>
        private static void SpawnSpell(GameObject spell, Vector3 position, int place)
        {
            var newSpell = Instantiate(spell, position, Quaternion.identity);
            newSpell.GetComponent<SpellData>().NestId = place;
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
                    SpawnSpell(_spellsTier2[spell], position, place);
                    break;
                case 2:
                    SpawnSpell(_spellsTier3[spell], position, place);
                    break;
                case 3:
                    _nests[place].GetComponent<OccupationStatusScript>().IsOccupied = false;
                    break;
            }
        }

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
            });
        }

        #endregion

        #endregion
    }
}
