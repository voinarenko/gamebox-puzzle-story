using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MergeController : MonoBehaviour, IPointerClickHandler
    {
        // Управление слияниями

        private SpellGenerator _spellGenerator;                                                    // Генератор заклинаний

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _spellGenerator = FindObjectOfType(typeof(SpellGenerator)) as SpellGenerator;
        }


        /// <summary>
        /// Метод обработки нажатия на левую клавишу мыши
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickMerge();
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
