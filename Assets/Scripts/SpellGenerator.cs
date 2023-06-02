using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class SpellGenerator : MonoBehaviour
    {
        #region ���������� ����������

        [SerializeField] private GameObject[] _nests = new GameObject[30];          // ������ �������� � ������������

        private bool _firstSelected;                              // ������� ����������� �������
        private string _firstSelectedTag;                         // ��� ����������� �������
        private int _firstSelectedTier;                           // ������� ����������� �������


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
                result = _nests[randomPosition].transform.position;
                placeIsFound = true;
            }
            return result;
        }

        private void FreeNest(GameObject target)
        {
            Collider[] colliders;
            if ((colliders = Physics.OverlapSphere(target.transform.position, 100f /* Radius */)).Length <= 1) //Presuming the object you are testing also has a collider 0 otherwise
            {
                Debug.Log(colliders.Length);
                return;
            }
            foreach (var c in colliders)
            {
                var go = c.gameObject; //This is the game object you collided with
                if (go == gameObject) continue; //Skip the object itself
                go.GetComponent<OccupationStatusScript>().IsOccupied = false; //Do something
            }
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
        public void NextTierSpell(Vector3 position)
        {
            var spell = FindSpell();
            switch (_firstSelectedTier)
            {
                case 1:
                    SpawnSpell(_spellsTier2[spell], position);
                    break;
                case 2:
                    SpawnSpell(_spellsTier3[spell], position);
                    break;
                case 3:
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
            _firstSelectedTier = target.GetComponent<TierLevel>().Tier;                         // ������� �������� �������
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
                NextTierSpell(moveTo);                                                        // ������ ����� ������ ������� ���� ������ ��������
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
