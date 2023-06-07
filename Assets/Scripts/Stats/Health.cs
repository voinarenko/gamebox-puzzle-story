using UnityEngine;

namespace Assets.Scripts.Stats
{
    public abstract class Health : MonoBehaviour
    {
        // «доровье

        #region ѕеременные

        [SerializeField] protected float MaxHealth;             // ћаксимальное здоровье
        [SerializeField] protected float CurrentHealth;         // “екущее здоровье
        [SerializeField] protected float HealAmount;            // ¬ход¤щее лечение
        [SerializeField] protected bool IsAlive;                // ќбъект жив

        #endregion


        /// <summary>
        /// ћетод получени¤ урона
        /// </summary>
        /// <param name="damage">количество урона</param>
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
        /// ћетод лечени¤
        /// </summary>
        /// <param name="healing">количество здоровь¤</param>
        public void Heal(float healing)
        {
            CurrentHealth += healing;
            CurrentHealth = CheckMaxHealth();
        }

        /// <summary>
        /// ћетод, возвращающий объЄм лечени¤
        /// </summary>
        /// <returns>объЄм лечени¤</returns>
        public float GetHealAmount()
        {
            return HealAmount;
        }

        /// <summary>
        /// ћетод проверки текущего здоровь¤ Ч не должно опускатьс¤ ниже 0
        /// </summary>
        private void CheckMinHealth()
        {
            if (CurrentHealth <= 0) SetCurrentHealthToZero(); 
        }

        /// <summary>
        /// ћетод проверки текущего здоровь¤ Ч не должно превышать максимальное
        /// </summary>
        /// <returns>текущее здоровье</returns>
        private float CheckMaxHealth()
        {
            if (CurrentHealth >= MaxHealth) CurrentHealth = MaxHealth;
            return CurrentHealth;
        }

        /// <summary>
        /// ћетод установки текущего здоровь¤ на 0
        /// </summary>
        private void SetCurrentHealthToZero()
        {
            CurrentHealth = 0;
        }

        /// <summary>
        /// ћетод проверки, жив ли игрок
        /// </summary>
        private void CheckIsAlive()
        {
            IsAlive = CurrentHealth > 0;
        }

        /// <summary>
        /// ћетод сброса здоровь¤
        /// </summary>
        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
