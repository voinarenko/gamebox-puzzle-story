using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MergeController : EventTrigger
    {
        // ���������� ���������


        #region ����������

        private GameObject _self;                                                                   // ��������������� ������
        private GameObject _target;                                                                 // ������� ������
        private SpellGenerator _spellGenerator;                                                     // ��������� ����������
        private bool _canBeMerged;                                                                  // ����� �����
        private float _offsetX, _offsetY;                                                           // ��������
        private Vector3 _startPosition;                                                             // ��������� ����������

        #endregion

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _spellGenerator = FindObjectOfType(typeof(SpellGenerator)) as SpellGenerator;
        }        
        
        /// <summary>
        /// ����� ��������� ������ �� ����� ������� ����
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
        }

        /// <summary>
        /// ����� ��������� ����������� ������� �� ����� ������ ����
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            _offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }

        /// <summary>
        /// ����� ��������� ��������������
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnDrag(PointerEventData eventData)
        {
            //_spellGenerator.TurnCollidersOff();
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!_spellGenerator.GetFirstSelected()) transform.position = new Vector3(mousePosition.x - _offsetX, mousePosition.y - _offsetY, 90);
        }

        /// <summary>
        /// ����� ��������� ���������� ����� ������
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer Up!");
            _spellGenerator.TurnCollidersOn();
            if (_canBeMerged)
            {
                _spellGenerator.Merge(_self, _target);
                _self = null;
                _target = null;
                _canBeMerged = false;
            }
            else
            {
                _spellGenerator.IsAnimating = true;
                _spellGenerator.TurnCollidersOff();
                transform.DOMove(_startPosition, 0.1f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    _spellGenerator.TurnCollidersOn();
                    _spellGenerator.IsAnimating = false;
                });
            }
            if (gameObject ==  null) return;
            ClickMerge();
        }

        /// <summary>
        /// ����� ��������� ������������
        /// </summary>
        /// <param name="collision">������� ������</param>
        public void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log("Triggered!");
            if (!collision.CompareTag(tag) ||
                GetComponent<SpellData>().Tier != collision.GetComponent<SpellData>().Tier) return;
            Debug.Log("Match!");
            _self = gameObject;
            _target = collision.gameObject;
            _canBeMerged = true;
        }

        /// <summary>
        /// ����� ��������� ������� ��������
        /// </summary>
        private void ClickMerge()
        {
            if (!_spellGenerator.GetFirstSelected())                                                                 // ���� ��� ���������� ��������
            {
                _spellGenerator.Select(gameObject);
            }
            else if (_spellGenerator.GetFirstSelected() && CompareTag("Selected"))                                   // ���� ������� ���������� ������
            {
                _spellGenerator.Deselect(gameObject);
            }
            else if (_spellGenerator.GetFirstSelected() &&                                                           // ���� ������ ��� �������
                     CompareTag(_spellGenerator.GetFirstSelectedTag()) &&                                            // ��� ��� ��������� � �������
                     gameObject.GetComponent<SpellData>().Tier == _spellGenerator.GetFirstSelectedTier())            // ��� ������� ��������� � �������
            {
                _spellGenerator.Merge(gameObject);
            }
        }

    }
}
