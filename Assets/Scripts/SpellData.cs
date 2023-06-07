using UnityEngine;

namespace Assets.Scripts
{
    public class SpellData : MonoBehaviour
    {
        // Параметры заклинания
            
        public int Tier;                    // уровень
        public int NestId;                  // номер позиции на поле
        public int MinProbabilityRange;     // минимальное значение диапазона вероятности
        public int MaxProbabilityRange;     // максимальное значение диапазона вероятности
    }
}
