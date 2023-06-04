using UnityEngine;

namespace Assets.Scripts.Stats
{
    public class DamageDealer : MonoBehaviour
    {
        // ��������� �����

        [SerializeField] private float _damage;        // ���������� ���������� �����

        /// <summary>
        /// �����, ������������ �������� �����
        /// </summary>
        /// <returns>�������� �����</returns>
        public float GetDamage()
        {
            return _damage;
        }
    }
}
