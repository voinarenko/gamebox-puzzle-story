using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ChestController : MonoBehaviour, IPointerClickHandler
    {
        // ���������� ��������

        private SpellGenerator _spellGenerator;                                                    // ��������� ����������

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            _spellGenerator = FindObjectOfType(typeof(SpellGenerator)) as SpellGenerator;
        }


        /// <summary>
        /// ����� ��������� ������� �� ����� ������� ����
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_spellGenerator.IsAnimating) ClickChest();
        }

        /// <summary>
        /// ����� ��������� �������� �������
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
