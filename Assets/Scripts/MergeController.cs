using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MergeController : MonoBehaviour, IPointerClickHandler
    {
        // ���������� ���������

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
            ClickMerge();
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
