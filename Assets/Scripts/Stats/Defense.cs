using UnityEngine;

namespace Assets.Scripts.Stats
{
    public abstract class Defense : MonoBehaviour
    {
        // Защита

        [SerializeField] protected float MinDefense;             // минимальное значение защиты
        [SerializeField] protected float MaxDefense = 100;       // максимальное значение защиты
        [SerializeField] protected float CurrentDefense;         // текущее значение защиты
        [SerializeField] protected float DefenseAmount;          // значение повышения защиты

        /// <summary>
        /// Метод повышения защиты
        /// </summary>
        /// <param name="value">значение защиты</param>
        public void SetDefense(float value)
        {
            CurrentDefense += value;
            if (CurrentDefense > MaxDefense) CurrentDefense = MaxDefense;
        }

        /// <summary>
        /// Метод, возвращающий текущее значение защиты
        /// </summary>
        /// <returns>значение защиты</returns>
        public float GetDefense()
        {
            return CurrentDefense;
        }

        /// <summary>
        /// Метод, возвращающий объём защиты
        /// </summary>
        /// <returns>значение защиты</returns>
        public float GetDefenseAmount()
        {
            return DefenseAmount;
        }

        /// <summary>
        /// Метод, сбрасывающий значение защиты
        /// </summary>
        public void ResetDefense()
        {
            CurrentDefense = MinDefense;
        }
    }
}
