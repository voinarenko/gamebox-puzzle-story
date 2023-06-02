using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class SpellGenerator : MonoBehaviour
    {
        #region ���������� ����������

        private GameObject[] _nests = new GameObject[30];                           // ������ �������� � ������������
        private static int _nestId;                                                    // ����� ������� � �������

        private bool _firstSelected;                                                // ������� ����������� �������
        private string _firstSelectedTag;                                           // ��� ����������� �������
        private int _firstSelectedTier;                                             // ������� ����������� �������


        #region ������� ����������

        [SerializeField] private GameObject[] _spellsTier1 = new GameObject[3];    // ������ ���������� 1-�� ������
        [SerializeField] private GameObject[] _spellsTier2 = new GameObject[3];    // ������ ���������� 2-�� ������
        [SerializeField] private GameObject[] _spellsTier3 = new GameObject[3];    // ������ ���������� 3-�� ������

        #endregion

        #endregion


        #region ������

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _nests = GameObject.FindGameObjectsWithTag("Nest");                     // ��������� ������ ���������
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
        
        }

        #region ������ ������ �������� ����������

        /// <summary>
        /// ����� ��������� �������� ������� ����������� �������
        /// </summary>
        /// <returns>������� ����������� �������</returns>
        public bool GetFirstSelected()
        {
            return _firstSelected;
        }

        /// <summary>
        /// ����� ��������� �������� ���� ����������� �������
        /// </summary>
        /// <returns>�������� ����</returns>
        public string GetFirstSelectedTag()
        {
            return _firstSelectedTag;
        }

        /// <summary>
        /// ����� ��������� �������� ������ ����������� �������
        /// </summary>
        /// <returns>�������� ������</returns>
        public int GetFirstSelectedTier()
        {
            return _firstSelectedTier;
        }

        #endregion

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
                _nestId = randomPosition;
                result = _nests[randomPosition].transform.position;
                placeIsFound = true;
            }
            return result;
        }

        /// <summary>
        /// �����, ��������� ����� ��������� ����� ����������� �������
        /// </summary>
        /// <param name="target">������������ ������</param>
        private void FreeNest(GameObject target)
        {
            _nests[target.GetComponent<SpellData>().NestId].GetComponent<OccupationStatusScript>().IsOccupied = false;
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
        /// <param name="place">������� � �������</param>
        private static void SpawnSpell(GameObject spell, Vector3 position, int place)
        {
            var newSpell = Instantiate(spell, position, Quaternion.identity);
            newSpell.GetComponent<SpellData>().NestId = place;
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
                SpawnSpell(_spellsTier1[spell], position, _nestId);
            }
        }

        /// <summary>
        /// ����� ����������� ���� ���������� �� ���� �������
        /// </summary>
        /// <returns>����� ������� � �������</returns>
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
        /// ����� �������� ���������� ���� �������, ��� ������
        /// </summary>
        /// <param name="position">���������� ��������</param>
        /// <param name="place">������� � �������</param>
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

        #region ������ ��������� ��������� � ������������

        /// <summary>
        /// ����� ��������� �������
        /// </summary>
        /// <param name="target">������</param>
        public void Select(GameObject target)
        {
            _firstSelectedTag =  target.tag;                                                    // ����������� ���������� ���������� ��� �������� �������
            _firstSelectedTier = target.GetComponent<SpellData>().Tier;                         // ������� �������� �������
            target.tag = "Selected";                                                            // ������ ��� �������� �������
            _firstSelected = true;                                                              // �������� ������� ����������� �������
            target.GetComponent<SpriteRenderer>().color = Color.yellow;                         // ������ ���� �������� �������
        }

        /// <summary>
        /// ����� ������ ��������� � �������
        /// </summary>
        /// <param name="target">������</param>
        public void Deselect(GameObject target)
        {
            target.tag = _firstSelectedTag;                                                     // ���������� ���
            _firstSelectedTag = null;                                                           // ���������� ��� ����������� �������
            _firstSelected = false;                                                             // ������� ������� ����������� �������
            target.GetComponent<SpriteRenderer>().color = Color.white;                          // ���������� ������� ����
        }

        /// <summary>
        /// ����� ������� ���� ���������� �������� � ����, ������� ����
        /// </summary>
        /// <param name="target">������</param>
        public void Merge(GameObject target)
        {
            var selected = GameObject.FindWithTag("Selected");                         // ������� ���������� ������
            Vector2 moveTo = target.transform.position;                                       // ����� ���������� ��������
            selected.transform.DOMove(moveTo, 0.5f).OnComplete(() =>                  // ��������� �������� �����������, �� ����������:
            {
                NextTierSpell(moveTo, target.GetComponent<SpellData>().NestId);               // ������ ����� ������ ������� ���� ������ ��������
                FreeNest(selected);
                Destroy(selected);                                                              // ���������� ������ ������
                Destroy(target);                                                                // � �������
                _firstSelectedTag = null;                                                       // ���������� ��� ����������� �������
                _firstSelected = false;                                                         // � ������� ����������� �������
            });
        }

        #endregion

        #endregion
    }
}
