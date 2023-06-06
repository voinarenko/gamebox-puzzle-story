using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MergeController : EventTrigger
    {
        // ���������� ���������


        #region ����������

        private SpellGenerator _spellGenerator;                                                     // ��������� ����������
        private bool _dragging;                                                                     // ��������� ��������������
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
            if (!_spellGenerator.IsAnimating && !_dragging) ClickMerge();
        }

        /// <summary>
        /// ����� ��������� ����������� ������� �� ����� ������� ����
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
            _dragging = true;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x - _offsetX, mousePosition.y - _offsetY, 90);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!_dragging) return;            
            transform.DOMove(_startPosition, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                _dragging = false;
            });
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
