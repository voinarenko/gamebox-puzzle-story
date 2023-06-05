using Assets.Scripts.Stats;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class SpellGenerator : MonoBehaviour
    {
        // ��������� ����������

        #region ���������� ����������

        public bool IsAnimating;                                                    // ��������� �������� ��� ���������� �������    

        [SerializeField] private GameObject _eventSystem;                           // ��������� �������

        private GameObject[] _nests = new GameObject[30];                           // ������ �������� � ������������
        private static int _nestId;                                                 // ����� ������� � �������

        private bool _firstSelected;                                                // ������� ����������� �������
        private string _firstSelectedTag;                                           // ��� ����������� �������
        private int _firstSelectedTier;                                             // ������� ����������� �������

        [SerializeField] private int _enemyMove = 3;                                // ��� �����
        private int _moveCounter;                                                   // ������� �����

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

        #region ������ ������ � �����

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
        /// �����, ��������� ������� ����
        /// </summary>
        public static void ClearTheField()
        {
            var spellsLeft = FindObjectsOfType<SpellData>();
            foreach ( var spell in spellsLeft) { Destroy(spell.gameObject); }

            var nestsLeft = FindObjectsOfType<OccupationStatusScript>();
            foreach (var nest in nestsLeft) { nest.IsOccupied = false; }
        }

        #endregion

        #region ������ ������ � ������������

        /// <summary>
        /// ����� ������ ���������� ���������� � ������������ � ��� �����
        /// </summary>
        /// <returns>��������� ������</returns>
        private int SelectRandomSpell()
        {
            var spellSelected = false;
            var result = 0;

            while (!spellSelected)
            {
                var rnd = new Random();
                var i = rnd.Next(100);      // ��������� �������� � ��������� �����
                for (var j = 0; j < _spellsTier1.Length; j++)
                {
                    // ��������� ��������� � �������� ����� ����������
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
        /// ����� ��������� ����������
        /// </summary>
        /// <param name="spell">���������� ������</param>
        /// <param name="position">����������</param>
        /// <param name="place">������� � �������</param>
        private void SpawnSpell(GameObject spell, Vector3 position, int place)
        {
            var newSpell = Instantiate(spell, position, Quaternion.identity);
            newSpell.GetComponent<SpellData>().NestId = place;
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
        /// �����, ����������� ��������, � ����������� �� ������� ����������
        /// </summary>
        /// <param name="spell"></param>
        public void PerformAction(int spell)
        {
            var player = GameObject.FindWithTag("Player");      // ���� ������
            var enemy = GameObject.FindWithTag("Enemy");        // ���� �����

            // ���� ����������
            switch (spell)
            { 
                case 0:     // �����
                    enemy.GetComponent<Health>().TakeDamage(player.GetComponent<DamageDealer>().GetDamage() * _firstSelectedTier);
                    break;
                case 1:     // ��������
                    player.GetComponent<Health>().Heal(player.GetComponent<Health>().GetHealAmount() * _firstSelectedTier);
                    break;
                case 2:     // ������
                    player.GetComponent<Defense>().SetDefense(player.GetComponent<Defense>().GetDefenseAmount() * _firstSelectedTier);
                    break;
            }
        }

        /// <summary>
        /// ����� �������� ��������� ������ � ������
        /// </summary>
        private void CheckStatus()
        {
            var player = GameObject.FindWithTag("Player");      // ���� ������
            var enemy = GameObject.FindWithTag("Enemy");        // ���� �����

            // ���� ���� ���������, ��������� ��������� ������
            if (enemy == null) { player.GetComponent<PlayerController>().SelectNextEpisode(); }
            else
            {
                // ������� ���� �� ����� �����
                if (_moveCounter == _enemyMove)
                {
                    player.GetComponent<Health>().TakeDamage(enemy.GetComponent<DamageDealer>().GetDamage());   // ������� ���� ������
                    _moveCounter = 0;

                    // ��������� ������������� ������
                    player = GameObject.FindWithTag("Player");

                    // ���� ����� ���������, ������� ����� ���������
                    if (player == null)
                    {
                        player.GetComponent<Health>().ResetHealth();
                        _eventSystem.GetComponent<GameManager>().DefeatedMenu();
                    }
                }
                else
                {
                    // ����������� �������
                    _moveCounter++;
                }
            }
        }

        #endregion

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
            IsAnimating = true;
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
                CheckStatus();
                IsAnimating = false;
            });
        }

        #endregion

        #endregion
    }
}
