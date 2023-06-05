using UnityEngine;

namespace Assets.Scripts.Stats
{
    public abstract class Health : MonoBehaviour
    {
        // ��������

        #region ����������

        [SerializeField] protected float MaxHealth;             // ������������ ��������
        [SerializeField] protected float CurrentHealth;         // ������� ��������
        [SerializeField] protected float HealAmount;            // �������� �������
        [SerializeField] protected bool IsAlive;                // ������ ���

        #endregion


        /// <summary>
        /// ����� ��������� �����
        /// </summary>
        /// <param name="damage">���������� �����</param>
        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage * ((100 - GetComponent<Defense>().GetDefense()) / 100);                        
            CheckMinHealth();
            CheckIsAlive();
            if (IsAlive) return;
            CurrentHealth = MaxHealth;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ����� �������
        /// </summary>
        /// <param name="healing">���������� ��������</param>
        public void Heal(float healing)
        {
            CurrentHealth += healing;
            CurrentHealth = CheckMaxHealth();
        }

        /// <summary>
        /// �����, ������������ ����� �������
        /// </summary>
        /// <returns>����� �������</returns>
        public float GetHealAmount()
        {
            return HealAmount;
        }

        /// <summary>
        /// ����� �������� �������� �������� � �� ������ ���������� ���� 0
        /// </summary>
        private void CheckMinHealth()
        {
            if (CurrentHealth <= 0) SetCurrentHealthToZero(); 
        }

        /// <summary>
        /// ����� �������� �������� �������� � �� ������ ��������� ������������
        /// </summary>
        /// <returns>������� ��������</returns>
        private float CheckMaxHealth()
        {
            if (CurrentHealth >= MaxHealth) CurrentHealth = MaxHealth;
            return CurrentHealth;
        }

        /// <summary>
        /// ����� ��������� �������� �������� �� 0
        /// </summary>
        private void SetCurrentHealthToZero()
        {
            CurrentHealth = 0;
        }

        /// <summary>
        /// ����� ��������, ��� �� �����
        /// </summary>
        private void CheckIsAlive()
        {
            IsAlive = CurrentHealth > 0;
        }

        /// <summary>
        /// ����� ������ ��������
        /// </summary>
        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
