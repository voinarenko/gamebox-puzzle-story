using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MergeController : EventTrigger
    {
        // Управление слияниями


        #region Переменные

        private SpellGenerator _spellGenerator;                                                     // Генератор заклинаний
        private bool _dragging;                                                                     // состояние перетаскивания
        private float _offsetX, _offsetY;                                                           // смещение
        private Vector3 _startPosition;                                                             // начальные координаты

        #endregion

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _spellGenerator = FindObjectOfType(typeof(SpellGenerator)) as SpellGenerator;
        }        
        
        /// <summary>
        /// Метод обработки щелчка по левой клавише мыши
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!_spellGenerator.IsAnimating && !_dragging) ClickMerge();
        }

        /// <summary>
        /// Метод обработки длительного нажатия на левую клавишу мыши
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            _offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }

        /// <summary>
        /// Метод обработки перетаскивания
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
        /// Метод обработки слияния объектов
        /// </summary>
        private void ClickMerge()
        {
            if (!_spellGenerator.GetFirstSelected())                                                                 // если нет выделенных объектов
            {
                _spellGenerator.Select(gameObject);
            }
            else if (_spellGenerator.GetFirstSelected() && CompareTag("Selected"))                                   // если выбрали выделенный объект
            {
                _spellGenerator.Deselect(gameObject);
            }
            else if (_spellGenerator.GetFirstSelected() &&                                                           // если объект уже выделен
                     CompareTag(_spellGenerator.GetFirstSelectedTag()) &&                                            // его тэг совпадает с текущим
                     gameObject.GetComponent<SpellData>().Tier == _spellGenerator.GetFirstSelectedTier())            // его уровень совпадает с текущим
            {
                _spellGenerator.Merge(gameObject);
            }
        }

    }
}
