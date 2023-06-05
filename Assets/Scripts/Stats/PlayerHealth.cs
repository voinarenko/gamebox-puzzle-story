using System.Globalization;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Stats
{
    public class PlayerHealth : Health
    {
        // Здоровье игрока

        #region Переменные

        [SerializeField] private TMP_Text _healthText;

        #endregion

        // Инициализация
        private void Awake()
        {
            MaxHealth = 150;            // Стартовое здоровье
            CurrentHealth = MaxHealth;  // Текущее здоровье
            IsAlive = true;             // Игрок жив
        }

        private void Update()
        {
            _healthText.text = CurrentHealth.ToString(CultureInfo.InvariantCulture);
            //if (!IsAlive) {PlayerDead();}
        }
    }
}
