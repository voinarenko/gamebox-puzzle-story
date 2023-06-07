using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MergeController : EventTrigger
    {
        // ”правление сли¤ни¤ми

        #region ѕеременные

        private GameObject _self;                                                                   // перетаскиваемый объект
        private GameObject _target;                                                                 // целевой объект
        private SpellGenerator _spellGenerator;                                                     // √енератор заклинаний
        private bool _canBeMerged;                                                                  // можно слить
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
        /// ћетод обработки щелчка по левой клавише мыши
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
        }

        /// <summary>
        /// ћетод обработки длительного нажати¤ на левой кнопке мыши
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            _offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }

        /// <summary>
        /// ћетод обработки перетаскивани¤
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnDrag(PointerEventData eventData)
        {
            //_spellGenerator.TurnCollidersOff();
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!_spellGenerator.GetFirstSelected()) transform.position = new Vector3(mousePosition.x - _offsetX, mousePosition.y - _offsetY, 90);
        }

        /// <summary>
        /// ћетод обработки отпускани¤ левой кнопки
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerUp(PointerEventData eventData)
        {
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
        /// ћетод обработки столкновений
        /// </summary>
        /// <param name="collision">целевой объект</param>
        public void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag(tag) ||
                GetComponent<SpellData>().Tier != collision.GetComponent<SpellData>().Tier) return;
            _self = gameObject;
            _target = collision.gameObject;
            _canBeMerged = true;
        }

        /// <summary>
        /// ћетод обработки сли¤ни¤ объектов
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
