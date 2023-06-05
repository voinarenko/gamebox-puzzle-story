using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ChestController : MonoBehaviour, IPointerClickHandler
    {
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
            if (!_spellGenerator.IsAnimating) ClickChest();
        }

        /// <summary>
        /// Метод обработки открытия сундука
        /// </summary>
        private void ClickChest()
        {
            _spellGenerator.FillTheField();
            Destroy(gameObject);
        }

    }
}
