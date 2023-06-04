using UnityEngine;

namespace Assets.Scripts.Stats
{
    public class DamageDealer : MonoBehaviour
    {
        // Нанесение урона

        [SerializeField] private float _damage;        // Переменная количества урона

        /// <summary>
        /// Метод, возвращающий значение урона
        /// </summary>
        /// <returns>значение урона</returns>
        public float GetDamage()
        {
            return _damage;
        }
    }
}
