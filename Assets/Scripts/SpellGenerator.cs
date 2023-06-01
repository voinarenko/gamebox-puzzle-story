using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class SpellGenerator : MonoBehaviour
    {
        #region Объявление переменных
        [SerializeField] private GameObject[] _nests = new GameObject[30];          // Массив объектов с координатами

        private readonly Quaternion _rotation = new(0,0,0,1);

        #region Массивы заклинаний

        [SerializeField] private GameObject[] _spellsLevel1 = new GameObject[3];    // Массив заклинаний 1-го уровня
        [SerializeField] private GameObject[] _spellsLevel2 = new GameObject[3];    // Массив заклинаний 2-го уровня
        [SerializeField] private GameObject[] _spellsLevel3 = new GameObject[3];    // Массив заклинаний 3-го уровня

        #endregion

        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            _nests = GameObject.FindGameObjectsWithTag("Nest");                     // Заполняем массив объектами
        }

        // Update is called once per frame
        private void Update()
        {
        
        }

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
                result = _nests[randomPosition].transform.position;
                placeIsFound = true;
            }
            return result;
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
            var result = rnd.Next(_spellsLevel1.Length);
            return result;
        }

        /// <summary>
        /// Метод генерации заклинания
        /// </summary>
        /// <param name="spell">вызываемый объект</param>
        /// <param name="position">координаты</param>
        /// <param name="rotation">поворот</param>
        private static void SpawnSpell(GameObject spell, Vector3 position, Quaternion rotation)
        {
            Instantiate(spell, position, rotation);
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
                SpawnSpell(_spellsLevel1[spell], position, _rotation);
            }
        }
    }
}
