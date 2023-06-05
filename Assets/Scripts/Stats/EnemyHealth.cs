using System.Globalization;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Stats
{
    public class EnemyHealth : Health
    {
        // Здоровье врага

        #region Переменные

        [SerializeField] private TMP_Text _healthText;

        #endregion

        // Инициализация
        private void Awake()
        {
            CurrentHealth = MaxHealth;  // Текущее здоровье
            IsAlive = true;             // Враг жив
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            _healthText.text = CurrentHealth.ToString(CultureInfo.InvariantCulture);
        }
    }
}
