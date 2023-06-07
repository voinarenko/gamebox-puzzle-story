using UnityEngine;

namespace Assets.Scripts.Stats
{
    public abstract class Defense : MonoBehaviour
    {
        // ������

        [SerializeField] protected float MinDefense;             // ����������� �������� ������
        [SerializeField] protected float MaxDefense = 100;       // ������������ �������� ������
        [SerializeField] protected float CurrentDefense;         // ������� �������� ������
        [SerializeField] protected float DefenseAmount;          // �������� ��������� ������

        /// <summary>
        /// ����� ��������� ������
        /// </summary>
        /// <param name="value">�������� ������</param>
        public void SetDefense(float value)
        {
            CurrentDefense += value;
            if (CurrentDefense > MaxDefense) CurrentDefense = MaxDefense;
        }

        /// <summary>
        /// �����, ������������ ������� �������� ������
        /// </summary>
        /// <returns>�������� ������</returns>
        public float GetDefense()
        {
            return CurrentDefense;
        }

        /// <summary>
        /// �����, ������������ ����� ������
        /// </summary>
        /// <returns>�������� ������</returns>
        public float GetDefenseAmount()
        {
            return DefenseAmount;
        }

        /// <summary>
        /// �����, ������������ �������� ������
        /// </summary>
        public void ResetDefense()
        {
            CurrentDefense = MinDefense;
        }
    }
}
