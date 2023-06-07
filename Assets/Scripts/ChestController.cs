﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ChestController : MonoBehaviour, IPointerClickHandler
    {
        // Управление сундуком

        private SpellGenerator _spellGenerator;                                                    // генератор заклинаний

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _spellGenerator = FindObjectOfType(typeof(SpellGenerator)) as SpellGenerator;
        }


        /// <summary>
        /// Метод обработки щелчка по левой кнопке мыши
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
            _spellGenerator.GenerateSpells();
            gameObject.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                Destroy(gameObject);
            });

        }

    }
}
