using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class SpellGenerator : MonoBehaviour
    {
        #region ���������� ����������
        [SerializeField] private GameObject[] _nests = new GameObject[30];          // ������ �������� � ������������
        [SerializeField] private int _maxSpellTier = 3;                             // ������������ ������� ����������

        public static bool FirstSelected;
        public static string FirstSelectedTag;

        #region ������� ����������

        [SerializeField] private GameObject[] _spellsTier1 = new GameObject[3];    // ������ ���������� 1-�� ������
        [SerializeField] private GameObject[] _spellsTier2 = new GameObject[3];    // ������ ���������� 2-�� ������
        [SerializeField] private GameObject[] _spellsTier3 = new GameObject[3];    // ������ ���������� 3-�� ������

        #endregion

        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            _nests = GameObject.FindGameObjectsWithTag("Nest");                     // ��������� ������ ���������
        }

        // Update is called once per frame
        private void Update()
        {
        
        }

        /// <summary>
        /// ����� ������ ���������� ����� �� ����
        /// </summary>
        /// <returns>���������� ���������� �����</returns>
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
        /// ����� �������� ������������� ����
        /// </summary>
        /// <returns>������������� ����</returns>
        private bool FieldIsFull()
        {
            var occupiedNests = _nests.Count(nest => nest.GetComponent<OccupationStatusScript>().IsOccupied);
            return occupiedNests == 30;
        }

        /// <summary>
        /// ����� �������� ��������� �����
        /// </summary>
        /// <param name="nest">����������� ������</param>
        /// <returns>��������� �����</returns>
        private static bool NestIsOccupied(GameObject nest)
        {
            return nest.GetComponent<OccupationStatusScript>().IsOccupied;
        }

        /// <summary>
        /// ����� ������ ���������� ����������
        /// </summary>
        /// <returns>��������� ������</returns>
        private int SelectRandomSpell()
        {
            var rnd = new Random();
            var result = rnd.Next(_spellsTier1.Length);
            return result;
        }

        /// <summary>
        /// ����� ��������� ����������
        /// </summary>
        /// <param name="spell">���������� ������</param>
        /// <param name="position">����������</param>
        /// <param name="rotation">�������</param>
        private static void SpawnSpell(GameObject spell, Vector3 position)
        {
            Instantiate(spell, position, Quaternion.identity);
        }

        /// <summary>
        /// ����� ���������� ���� ������������
        /// </summary>
        public void FillTheField()
        {
            while (!FieldIsFull())
            {
                var spell = SelectRandomSpell();
                var position = FindSpawnPlace();
                SpawnSpell(_spellsTier1[spell], position);
            }
        }

        /// <summary>
        /// �����, ������������ ������������ ������� ����������
        /// </summary>
        /// <returns>������������ �������</returns>
        public int MaxLevelTier()
        {
            return _maxSpellTier;
        }
    }
}
