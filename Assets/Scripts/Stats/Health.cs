using UnityEngine;

namespace Assets.Scripts.Stats
{
    public abstract class Health : MonoBehaviour
    {
        // Здоровье

        #region Переменные

        [SerializeField] protected float MaxHealth;             // Максимальное здоровье
        [SerializeField] protected float CurrentHealth;         // Текущее здоровье
        [SerializeField] protected float HealAmount;            // Входящее лечение
        [SerializeField] protected bool IsAlive;                // Объект жив

        #endregion


        /// <summary>
        /// Метод получения урона
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
        /// Метод лечения
        /// </summary>
        /// <param name="healing">количество здоровья</param>
        public void Heal(float healing)
        {
            CurrentHealth += healing;
            CurrentHealth = CheckMaxHealth();
        }

        /// <summary>
        /// Метод, возвращающий объём лечения
        /// </summary>
        /// <returns>объём лечения</returns>
        public float GetHealAmount()
        {
            return HealAmount;
        }

        /// <summary>
        /// Метод проверки текущего здоровья — не должно опускаться ниже 0
        /// </summary>
        private void CheckMinHealth()
        {
            if (CurrentHealth <= 0) SetCurrentHealthToZero(); 
        }

        /// <summary>
        /// Метод проверки текущего здоровья — не должно превышать максимальное
        /// </summary>
        /// <returns>текущее здоровье</returns>
        private float CheckMaxHealth()
        {
            if (CurrentHealth >= MaxHealth) CurrentHealth = MaxHealth;
            return CurrentHealth;
        }

        /// <summary>
        /// Метод установки текущего здоровья на 0
        /// </summary>
        private void SetCurrentHealthToZero()
        {
            CurrentHealth = 0;
        }

        /// <summary>
        /// Метод проверки, жив ли игрок
        /// </summary>
        private void CheckIsAlive()
        {
            IsAlive = CurrentHealth > 0;
        }

        /// <summary>
        /// Метод сброса здоровья
        /// </summary>
        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
