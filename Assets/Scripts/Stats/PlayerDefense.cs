using UnityEngine;

namespace Assets.Scripts.Stats
{
    public class PlayerDefense : Defense
    {
        [SerializeField] private GameObject[] _images;

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            switch (CurrentDefense)
            {
                case >= 0 and <= 30:
                    _images[0].SetActive(true);
                    _images[1].SetActive(false);
                    _images[2].SetActive(false);
                    break;
                case >= 31 and <= 60:
                    _images[0].SetActive(false);
                    _images[1].SetActive(true);
                    _images[2].SetActive(false);
                    break;
                case >= 61 and <= 100:
                    _images[0].SetActive(false);
                    _images[1].SetActive(false);
                    _images[2].SetActive(true);
                    break;
            }
        }
    }
}
